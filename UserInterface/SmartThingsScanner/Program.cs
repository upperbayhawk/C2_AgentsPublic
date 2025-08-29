// .NET Framework 4.8
// NuGet: MQTTnet 3.0.12, Newtonsoft.Json 11.0.2

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StToMqttFx_Mqtt3_Auto_NJ11_Delta
{
    internal class Program
    {
        // Cache of last published fingerprint per topic (value+unit). Survives the whole run.
        private static readonly ConcurrentDictionary<string, string> LastPublished = new ConcurrentDictionary<string, string>(StringComparer.Ordinal);

        static int Main(string[] args)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            try
            {
                RunAsync().GetAwaiter().GetResult();
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Fatal: " + ex);
                return 1;
            }
        }

        static async Task RunAsync()
        {
            var cfg = Config.LoadFromEnv();
            var api = new SmartThingsApi(cfg.Token);

            await AutoDiscoverAsync(api, cfg);

            using (var mqtt = await ConnectMqttAsync(cfg))
            {
                var firstLoop = true;

                while (true)
                {
                    Console.WriteLine("Fetching devices" + (string.IsNullOrEmpty(cfg.LocationId) ? "" : $" (location {cfg.LocationId})") + "…");
                    var devices = await api.ListDevicesAsync(cfg.LocationId);

                    if (!string.IsNullOrEmpty(cfg.HubId))
                        devices = devices.Where(d => string.Equals(d.HubId, cfg.HubId, StringComparison.OrdinalIgnoreCase)).ToList();

                    Console.WriteLine("Devices: " + devices.Count);

                    var results = new List<Tuple<StDevice, JObject>>();
                    using (var sem = new SemaphoreSlim(cfg.Parallelism))
                    {
                        var tasks = devices.Select(async d =>
                        {
                            await sem.WaitAsync().ConfigureAwait(false);
                            try
                            {
                                var st = await api.GetDeviceStatusAsync(d.DeviceId).ConfigureAwait(false);
                                lock (results) results.Add(Tuple.Create(d, st));
                            }
                            finally { sem.Release(); }
                        });
                        await Task.WhenAll(tasks).ConfigureAwait(false);
                    }

                    var publishedCount = 0;
                    foreach (var tup in results.OrderBy(r => r.Item1.Label ?? r.Item1.Name))
                        publishedCount += await PublishStatusDeltasAsync(mqtt, cfg, tup.Item1, tup.Item2, firstLoop).ConfigureAwait(false);

                    Console.WriteLine(firstLoop
                        ? $"Initial snapshot published: {publishedCount} messages."
                        : $"Delta publish complete: {publishedCount} changes.");

                    firstLoop = false;

                    if (cfg.WatchSeconds <= 0) break;
                    await Task.Delay(TimeSpan.FromSeconds(cfg.WatchSeconds)).ConfigureAwait(false);
                }

                await mqtt.DisconnectAsync().ConfigureAwait(false);
            }
        }

        // ---- Auto-discovery ----
        static async Task AutoDiscoverAsync(SmartThingsApi api, Config cfg)
        {
            if (string.IsNullOrWhiteSpace(cfg.LocationId))
            {
                var locs = await api.ListLocationsAsync();
                if (locs.Count == 0)
                    throw new InvalidOperationException("No SmartThings locations found for this token.");

                Console.WriteLine("Available locations:");
                foreach (var l in locs)
                    Console.WriteLine($"  {l.Name ?? "(unnamed)"}  {l.LocationId}");

                cfg.LocationId = locs[0].LocationId;
                Console.WriteLine($"Using locationId: {cfg.LocationId}  (set SMARTTHINGS_LOCATION_ID to override)");
            }

            if (string.IsNullOrWhiteSpace(cfg.HubId))
            {
                var devs = await api.ListDevicesAsync(cfg.LocationId);
                var hubs = devs.Select(d => d.HubId).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();

                if (hubs.Count == 1)
                {
                    cfg.HubId = hubs[0];
                    Console.WriteLine($"Using hubId: {cfg.HubId}  (set SMARTTHINGS_HUB_ID to override)");
                }
                else if (hubs.Count > 1)
                {
                    Console.WriteLine("Multiple hubs detected in this location:");
                    foreach (var h in hubs) Console.WriteLine("  " + h);
                    Console.WriteLine("Proceeding with NO hub filter. (Set SMARTTHINGS_HUB_ID to narrow it.)");
                }
                else
                {
                    Console.WriteLine("No hubId values found on devices; proceeding without hub filter.");
                }
            }
        }

        // ---------- MQTT (MQTTnet 3.0.12) ----------
        static async Task<IMqttClient> ConnectMqttAsync(Config cfg)
        {
            var factory = new MqttFactory();
            var client = factory.CreateMqttClient();

            var optsBuilder = new MQTTnet.Client.Options.MqttClientOptionsBuilder()
                .WithTcpServer(cfg.MqttHost, cfg.MqttPort)
                .WithClientId("st-mqtt-" + Guid.NewGuid().ToString("N"))
                .WithCleanSession();

            if (!string.IsNullOrWhiteSpace(cfg.MqttUser))
                optsBuilder = optsBuilder.WithCredentials(cfg.MqttUser, cfg.MqttPass ?? "");

            var options = optsBuilder.Build();
            await client.ConnectAsync(options, CancellationToken.None).ConfigureAwait(false);
            Console.WriteLine("MQTT connected to " + cfg.MqttHost + ":" + cfg.MqttPort);
            return client;
        }

        // ---------- Delta publishing ----------
        static async Task<int> PublishStatusDeltasAsync(IMqttClient mqtt, Config cfg, StDevice dev, JObject statusRoot, bool firstLoop)
        {
            var components = statusRoot["components"] as JObject;
            if (components == null) return 0;

            var count = 0;
            foreach (var compProp in components.Properties())
            {
                var compName = compProp.Name;
                var capsObj = compProp.Value as JObject;
                if (capsObj == null) continue;

                foreach (var capProp in capsObj.Properties())
                {
                    var capName = capProp.Name;
                    var attrsObj = capProp.Value as JObject;
                    if (attrsObj == null) continue;

                    foreach (var attrProp in attrsObj.Properties())
                    {
                        var attrName = attrProp.Name;
                        var attr = attrProp.Value as JObject;
                        if (attr == null) continue;

                        var valueToken = attr["value"];
                        if (valueToken == null) continue;
                        var unitTok = attr["unit"];
                        var unitStr = unitTok != null ? (string)unitTok : null;

                        var location = Safe(dev.LocationId) ?? "noloc";
                        var room = Safe(dev.RoomId) ?? "noroom";
                        var device = Safe(dev.Label ?? dev.Name ?? dev.DeviceId);
                        var comp = Safe(compName);
                        var cap = Safe(capName);
                        var at = Safe(attrName);

                        if ((device.Contains("XDATA")) && ((at =="humidity" || (at == "temperature") || (at == "power") || (at == "energy"))))
                        {
                            var topic = string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}",
                                cfg.BaseTopic, location, room, device, comp, cap, at);

                            // Fingerprint is only (value,unit) — NOT timestamp — so we detect real changes.
                            var fingerprint = BuildFingerprint(valueToken, unitStr);

                            // On the first loop: publish only if cfg.PublishInitial == true; otherwise seed the cache and skip
                            if (firstLoop && !cfg.PublishInitial)
                            {
                                LastPublished[topic] = fingerprint;
                                continue;
                            }

                            // If the value didn't change, skip
                            if (LastPublished.TryGetValue(topic, out var prev) && string.Equals(prev, fingerprint, StringComparison.Ordinal))
                                continue;

                            // Build payload with timestamp (this changes every publish, but doesn't affect change detection)
                            var payload = new MqttPayload
                            {
                                DeviceId = dev.DeviceId,
                                DeviceName = dev.Label ?? dev.Name,
                                Component = compName,
                                Capability = capName,
                                Attribute = attrName,
                                Value = valueToken,
                                Unit = unitStr,
                                Ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                            };

                            var json = JsonConvert.SerializeObject(payload, Formatting.None);
                            var message = new MqttApplicationMessageBuilder()
                                .WithTopic(topic)
                                .WithPayload(json)
                                .WithQualityOfServiceLevel(cfg.QoS)
                                .WithRetainFlag(cfg.Retain)
                                .Build();

                            await mqtt.PublishAsync(message, CancellationToken.None).ConfigureAwait(false);
                            LastPublished[topic] = fingerprint;
                            count++;

                            Console.WriteLine(attrName + " -> " + valueToken);

                            Console.WriteLine(topic + " <- " + json);
                            //Console.WriteLine(topic + " <- " + (json.Length <= 120 ? json : json.Substring(0, 117) + "..."));
                        }
                    }
                }
            }
            return count;
        }

        static string BuildFingerprint(JToken valueToken, string unit)
        {
            // Normalize JToken kind (e.g., numbers vs strings) and include unit if present
            if (valueToken == null) return unit ?? "";
            var type = valueToken.Type;
            string v;
            switch (type)
            {
                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.Boolean:
                    v = valueToken.ToString(Newtonsoft.Json.Formatting.None);
                    break;
                case JTokenType.String:
                    v = ((string)valueToken) ?? "";
                    break;
                default:
                    // For objects/arrays, compact to canonical JSON (rare for ST attribute values)
                    v = valueToken.ToString(Newtonsoft.Json.Formatting.None);
                    break;
            }
            return unit == null ? v : (v + "|" + unit);
        }

        static string Safe(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return s.Trim()
                    .Replace(' ', '_')
                    .Replace('/', '-')
                    .Replace('\\', '-')
                    .Replace('#', '-')
                    .Replace('+', '-')
                    .Replace('$', '-');
        }
    }

    // ---------- Config ----------
    internal sealed class Config
    {
        public string Token { get; set; }
        public string LocationId { get; set; }
        public string HubId { get; set; }

        public string MqttHost { get; set; }
        public int MqttPort { get; set; }
        public string MqttUser { get; set; }
        public string MqttPass { get; set; }
        public string BaseTopic { get; set; }
        public MqttQualityOfServiceLevel QoS { get; set; }
        public bool Retain { get; set; }

        public int WatchSeconds { get; set; }
        public int Parallelism { get; set; }
        public bool PublishInitial { get; set; }

        public static Config LoadFromEnv()
        {
            string Required(string k)
            {
                var v = Environment.GetEnvironmentVariable(k);
                if (string.IsNullOrWhiteSpace(v)) throw new InvalidOperationException("Missing env var " + k);
                return v;
            }
            string Optional(string k) { return Environment.GetEnvironmentVariable(k); }

            var qosInt = 0; int.TryParse(Optional("MQTT_QOS"), out qosInt);
            if (qosInt < 0 || qosInt > 2) qosInt = 0;

            return new Config
            {
                Token = Required("SMARTTHINGS_TOKEN"),
                LocationId = Optional("SMARTTHINGS_LOCATION_ID"),
                HubId = Optional("SMARTTHINGS_HUB_ID"),
                MqttHost = Optional("MQTT_HOST") ?? "127.0.0.1",
                MqttPort = ParseInt(Optional("MQTT_PORT"), 1883),
                MqttUser = Optional("MQTT_USER"),
                MqttPass = Optional("MQTT_PASS"),
                BaseTopic = Optional("MQTT_BASE") ?? "smartthings",
                QoS = (MqttQualityOfServiceLevel)qosInt,
                Retain = ParseBool(Optional("MQTT_RETAIN"), true),
                WatchSeconds = ParseInt(Optional("WATCH_SECONDS"), 30),  // default to 30s so deltas make sense
                Parallelism = Math.Max(1, ParseInt(Optional("PARALLELISM"), 6)),
                PublishInitial = ParseBool(Optional("PUBLISH_INITIAL"), true), // publish baseline once by default
            };
        }

        static int ParseInt(string s, int defVal) { int v; return int.TryParse(s, out v) ? v : defVal; }
        static bool ParseBool(string s, bool defVal) { bool v; return bool.TryParse(s, out v) ? v : defVal; }
    }

    // ---------- SmartThings REST (patched readers) ----------
    internal sealed class SmartThingsApi
    {
        private readonly HttpClient _http;

        public SmartThingsApi(string token)
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri("https://api.smartthings.com/v1/"),
                Timeout = TimeSpan.FromMinutes(2)
            };
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<LocationInfo>> ListLocationsAsync()
        {
            using (var resp = await _http.GetAsync("locations").ConfigureAwait(false))
            {
                await EnsureSuccessOrThrowAsync(resp).ConfigureAwait(false);
                var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                JToken tok;
                try { tok = JToken.Parse(json); }
                catch
                {
                    var preview = json.Length > 400 ? json.Substring(0, 400) + "..." : json;
                    throw new HttpRequestException("GET /locations returned non-JSON. Preview:\n" + preview);
                }

                JArray arr = null;
                if (tok is JArray ja) arr = ja;
                else if (tok is JObject jo)
                {
                    if (jo["items"] is JArray items) arr = items;
                    else
                    {
                        var anyArrayProp = jo.Properties().FirstOrDefault(p => p.Value is JArray);
                        if (anyArrayProp != null) arr = (JArray)anyArrayProp.Value;
                    }
                }

                if (arr == null)
                    throw new HttpRequestException("GET /locations unexpected shape:\n" + tok.ToString(Formatting.Indented));

                var list = new List<LocationInfo>();
                foreach (var el in arr)
                {
                    list.Add(new LocationInfo
                    {
                        LocationId = (string)el["locationId"],
                        Name = (string)el["name"]
                    });
                }
                return list;
            }
        }

        public async Task<List<StDevice>> ListDevicesAsync(string locationId)
        {
            var baseUrl = "devices";
            var url = string.IsNullOrWhiteSpace(locationId) ? baseUrl
                : baseUrl + "?locationId=" + Uri.EscapeDataString(locationId);

            try
            {
                return await ListDevicesPagedAsync(url).ConfigureAwait(false);
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("422"))
            {
                Console.WriteLine("WARN: devices?locationId=... returned 422. Retrying without filter.");
                return await ListDevicesPagedAsync(baseUrl).ConfigureAwait(false);
            }
        }

        private async Task<List<StDevice>> ListDevicesPagedAsync(string url)
        {
            var items = new List<StDevice>();
            while (true)
            {
                using (var resp = await _http.GetAsync(url).ConfigureAwait(false))
                {
                    await EnsureSuccessOrThrowAsync(resp).ConfigureAwait(false);
                    var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                    JToken doc;
                    try { doc = JToken.Parse(json); }
                    catch
                    {
                        var preview = json.Length > 400 ? json.Substring(0, 400) + "..." : json;
                        throw new HttpRequestException("GET /devices returned non-JSON. Preview:\n" + preview);
                    }

                    JArray arr = null;
                    if (doc is JObject djo)
                    {
                        arr = djo["items"] as JArray;
                        if (arr == null)
                        {
                            var anyArrayProp = djo.Properties().FirstOrDefault(p => p.Value is JArray);
                            if (anyArrayProp != null) arr = (JArray)anyArrayProp.Value;
                        }
                    }
                    else if (doc is JArray dja)
                    {
                        arr = dja;
                    }

                    if (arr == null)
                        throw new HttpRequestException("GET /devices unexpected shape:\n" + doc.ToString(Formatting.Indented));

                    foreach (var el in arr)
                    {
                        items.Add(new StDevice
                        {
                            DeviceId = (string)el["deviceId"],
                            Name = (string)el["name"],
                            Label = (string)el["label"],
                            LocationId = (string)el["locationId"],
                            RoomId = (string)el["roomId"],
                            HubId = (string)el["hubId"]
                        });
                    }

                    string href = null;
                    if (doc is JObject djo2)
                    {
                        var hrefTok = djo2.SelectToken("_links.next.href");
                        href = hrefTok != null ? (string)hrefTok : null;
                    }
                    if (!string.IsNullOrEmpty(href))
                        url = href.Replace("https://api.smartthings.com/v1/", "");
                    else
                        break;
                }
            }
            return items;
        }

        public async Task<JObject> GetDeviceStatusAsync(string deviceId)
        {
            using (var resp = await _http.GetAsync("devices/" + deviceId + "/status").ConfigureAwait(false))
            {
                if (!resp.IsSuccessStatusCode)
                {
                    var body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Console.WriteLine($"WARN: status failed for {deviceId}: {(int)resp.StatusCode} {resp.ReasonPhrase}\n{body}");
                    return new JObject { ["components"] = new JObject() };
                }
                var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JObject.Parse(json);
            }
        }

        private static async Task EnsureSuccessOrThrowAsync(HttpResponseMessage resp)
        {
            if (resp.IsSuccessStatusCode) return;
            var body = resp.Content != null ? await resp.Content.ReadAsStringAsync().ConfigureAwait(false) : "";
            throw new HttpRequestException(
                $"HTTP {(int)resp.StatusCode} {resp.ReasonPhrase} for {resp.RequestMessage?.RequestUri}\n{body}");
        }
    }

    // ---------- Models ----------
    internal sealed class LocationInfo
    {
        public string LocationId { get; set; }
        public string Name { get; set; }
    }

    internal sealed class StDevice
    {
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string LocationId { get; set; }
        public string RoomId { get; set; }
        public string HubId { get; set; }
    }

    internal sealed class MqttPayload
    {
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string Component { get; set; }
        public string Capability { get; set; }
        public string Attribute { get; set; }
        public JToken Value { get; set; }  // preserves numeric vs string
        public string Unit { get; set; }
        public long Ts { get; set; }
    }
}

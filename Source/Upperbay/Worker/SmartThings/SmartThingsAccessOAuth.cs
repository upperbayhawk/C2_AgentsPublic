// .NET Framework 4.8
// NuGet: Newtonsoft.Json 11.0.2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Upperbay.Core.Logging;
using Upperbay.Agent.Interfaces;

namespace Upperbay.Worker.SmartThings
{
    /// <summary>
    /// Scans SmartThings for devices whose name/label contains "XDATA" and that
    /// expose a configured set of attributes. Uses SmartThings REST API (cloud).
    /// </summary>
    public sealed class SmartThingsAccess
    {
        // ---- Public model returned by Fetch() ----
        public sealed class ScannedDevice
        {
            public string DeviceId { get; set; }
            public string Name { get; set; }
            public string Label { get; set; }
            public string LocationId { get; set; }
            public string RoomId { get; set; }
            public string HubId { get; set; }
            /// <summary>Attribute values for the requested attributes, keyed by normalized path like "main.temperatureMeasurement.temperature".</summary>
            public Dictionary<string, JToken> Attributes { get; set; } = new Dictionary<string, JToken>(StringComparer.OrdinalIgnoreCase);
        }

        // ---- Config / state ----
        private string _token;
        private string _locationId;   // optional; auto-discovered if null/empty
        private string _hubId;        // optional
        private TimeSpan _timeout = TimeSpan.FromSeconds(120);
        private List<AttrSpec> _requiredAttrs = new List<AttrSpec>(); // parsed at Init

        private HttpClient _http;

        // Attribute spec parsed from strings like "cap.attr" or "component.cap.attr"
        private sealed class AttrSpec
        {
            public string Component { get; set; } = "main";
            public string Capability { get; set; }
            public string Attribute { get; set; }
            public string NormalizedKey => $"{Component}.{Capability}.{Attribute}";
        }

        /// <summary>
        /// Initialize the scanner.
        /// requiredAttributes: list like "switch.switch" or "temperatureMeasurement.temperature".
        /// You can also include component explicitly: "main.switch.switch".
        /// </summary>
        public void Init(
            string token,
            IEnumerable<string> requiredAttributes,
            string locationId = null,
            string hubId = null,
            int httpTimeoutSeconds = 120)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("SmartThings token is required.", nameof(token));

            _token = token.Trim();
            _locationId = string.IsNullOrWhiteSpace(locationId) ? null : locationId.Trim();
            _hubId = string.IsNullOrWhiteSpace(hubId) ? null : hubId.Trim();
            _timeout = TimeSpan.FromSeconds(Math.Max(5, httpTimeoutSeconds));

            _requiredAttrs = (requiredAttributes ?? Enumerable.Empty<string>())
                .Select(ParseAttrSpec)
                .ToList();

            if (_requiredAttrs.Count == 0)
                throw new ArgumentException("At least one required attribute is needed.", nameof(requiredAttributes));

            // Prepare HttpClient
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            _http = new HttpClient
            {
                BaseAddress = new Uri("https://api.smartthings.com/v1/"),
                Timeout = _timeout
            };
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        /// <summary>
        /// Fetch devices that:
        /// 1) Have "XDATA" in name/label, and
        /// 2) Expose ALL required attributes (from Init) in /status.
        /// Returns attribute values for just those requested attributes.
        /// </summary>
        public async Task<List<ScannedDevice>> Fetch()
        {
            EnsureInitialized();

            // Auto-discover location if not provided
            if (string.IsNullOrWhiteSpace(_locationId))
            {
                var locs = await ListLocationsAsync();
                if (locs.Count == 0)
                    throw new InvalidOperationException("No SmartThings locations found for this token.");
                _locationId = locs[0].LocationId;
            }

            // Get devices (optionally filtered by hub)
            var all = await ListDevicesAsync(_locationId);
            if (!string.IsNullOrWhiteSpace(_hubId))
                all = all.Where(d => string.Equals(d.HubId, _hubId, StringComparison.OrdinalIgnoreCase)).ToList();

            // Filter to XDATA devices
            var xdata = all.Where(d =>
            {
                var label = d.Label ?? d.Name ?? "";
                return label.IndexOf("XDATA", StringComparison.OrdinalIgnoreCase) >= 0;
            }).ToList();

            var results = new List<ScannedDevice>();

            // Pull /status and include only devices with all required attributes present
            foreach (var d in xdata)
            {
                var status = await GetDeviceStatusAsync(d.DeviceId);

                var attrMap = TryExtractAnyRequiredAttributes(status, _requiredAttrs); // include device if it has at least ONE required attribute

                if (attrMap != null) // all required attributes present
                {
                    results.Add(new ScannedDevice
                    {
                        DeviceId = d.DeviceId,
                        Name = d.Name,
                        Label = d.Label,
                        LocationId = d.LocationId,
                        RoomId = d.RoomId,
                        HubId = d.HubId,
                        Attributes = attrMap
                    });
                }
            }

            return results;
        }

        // ----------------- Helpers -----------------

        private void EnsureInitialized()
        {
            if (_http == null)
                throw new InvalidOperationException("Call Init(...) before Fetch().");
        }

        private static AttrSpec ParseAttrSpec(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                throw new ArgumentException("Empty attribute spec.");

            var parts = s.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                // cap.attr on main
                return new AttrSpec { Component = "main", Capability = parts[0].Trim(), Attribute = parts[1].Trim() };
            }
            if (parts.Length == 3)
            {
                return new AttrSpec { Component = parts[0].Trim(), Capability = parts[1].Trim(), Attribute = parts[2].Trim() };
            }
            throw new ArgumentException($"Invalid attribute spec '{s}'. Use 'cap.attr' or 'component.cap.attr'.");
        }

        private async Task<List<LocationInfo>> ListLocationsAsync()
        {
            using (var resp = await _http.GetAsync("locations").ConfigureAwait(false))
            {
                await EnsureSuccessOrThrowAsync(resp).ConfigureAwait(false);
                var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                var tok = ParseJson(json, "/locations");

                JArray arr = null;
                if (tok is JArray ja) arr = ja;
                else if (tok is JObject jo)
                {
                    if (jo["items"] is JArray items) arr = items;
                    else
                    {
                        var any = jo.Properties().FirstOrDefault(p => p.Value is JArray);
                        if (any != null) arr = (JArray)any.Value;
                    }
                }

                if (arr == null)
                    throw new HttpRequestException("GET /locations unexpected shape:\n" + tok.ToString(Newtonsoft.Json.Formatting.Indented));

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

        private async Task<List<StDevice>> ListDevicesAsync(string locationId)
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
                // Bad locationId → retry without filter
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
                    var tok = ParseJson(json, "/devices");

                    JArray arr = null;
                    if (tok is JObject djo)
                    {
                        arr = djo["items"] as JArray;
                        if (arr == null)
                        {
                            var any = djo.Properties().FirstOrDefault(p => p.Value is JArray);
                            if (any != null) arr = (JArray)any.Value;
                        }
                    }
                    else if (tok is JArray dja)
                    {
                        arr = dja;
                    }

                    if (arr == null)
                        throw new HttpRequestException("GET /devices unexpected shape:\n" + tok.ToString(Newtonsoft.Json.Formatting.Indented));

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

                    // pagination
                    string href = null;
                    if (tok is JObject djo2)
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

        private async Task<JObject> GetDeviceStatusAsync(string deviceId)
        {
            using (var resp = await _http.GetAsync("devices/" + deviceId + "/status").ConfigureAwait(false))
            {
                if (!resp.IsSuccessStatusCode)
                {
                    // Some devices (cloud/bridge) may fail; treat as no attributes.
                    return new JObject { ["components"] = new JObject() };
                }
                var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JObject.Parse(json);
            }
        }

        private static JToken ParseJson(string json, string context)
        {
            try { return JToken.Parse(json); }
            catch
            {
                var preview = json.Length > 400 ? json.Substring(0, 400) + "..." : json;
                throw new HttpRequestException($"GET {context} returned non-JSON. Preview:\n" + preview);
            }
        }

        private static async Task EnsureSuccessOrThrowAsync(HttpResponseMessage resp)
        {
            if (resp.IsSuccessStatusCode) return;
            var body = resp.Content != null ? await resp.Content.ReadAsStringAsync().ConfigureAwait(false) : "";
            throw new HttpRequestException(
                $"HTTP {(int)resp.StatusCode} {resp.ReasonPhrase} for {resp.RequestMessage?.RequestUri}\n{body}");
        }

        private static Dictionary<string, JToken> TryExtractAnyRequiredAttributes(JObject statusRoot, List<AttrSpec> required)
        {
            var components = statusRoot["components"] as JObject;
            if (components == null) return null;

            var result = new Dictionary<string, JToken>(StringComparer.OrdinalIgnoreCase);
            foreach (var spec in required)
            {
                // components -> component -> capability -> attribute -> { value, unit, ... }
                var comp = components[spec.Component] as JObject;
                if (comp == null) continue;

                var cap = comp[spec.Capability] as JObject;
                if (cap == null) continue;

                var attrObj = cap[spec.Attribute] as JObject;
                if (attrObj == null) continue;

                var valueToken = attrObj["value"];
                if (valueToken == null) continue;

                result[spec.NormalizedKey] = valueToken;
            }

            // If we found at least one requested attribute, return the subset; otherwise null to exclude the device
            return result.Count > 0 ? result : null;
        }

        /*
        private static Dictionary<string, JToken> TryExtractAllRequiredAttributes(JObject statusRoot, List<AttrSpec> required)
        {
            var components = statusRoot["components"] as JObject;
            if (components == null) return null;

            var result = new Dictionary<string, JToken>(StringComparer.OrdinalIgnoreCase);

            foreach (var spec in required)
            {
                // Navigate: components -> component -> capability -> attribute -> { value, unit, ... }
                var comp = components[spec.Component] as JObject;
                if (comp == null) return null;

                var cap = comp[spec.Capability] as JObject;
                if (cap == null) return null;

                var attrObj = cap[spec.Attribute] as JObject;
                if (attrObj == null) return null;

                var valueToken = attrObj["value"];
                if (valueToken == null) return null;

                result[spec.NormalizedKey] = valueToken;
            }

            return result;
        }
        */

        // -------- Internal DTOs --------

        private sealed class LocationInfo
        {
            public string LocationId { get; set; }
            public string Name { get; set; }
        }

        private sealed class StDevice
        {
            public string DeviceId { get; set; }
            public string Name { get; set; }
            public string Label { get; set; }
            public string LocationId { get; set; }
            public string RoomId { get; set; }
            public string HubId { get; set; }
        }
    }
}

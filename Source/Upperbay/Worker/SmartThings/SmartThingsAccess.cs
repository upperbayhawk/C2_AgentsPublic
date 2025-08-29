// .NET Framework 4.8
// NuGet: Newtonsoft.Json 11.0.2

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

// Keep your existing logging/interfaces if you want; not required here
using Upperbay.Core.Logging;
// using Upperbay.Agent.Interfaces;

namespace Upperbay.Worker.SmartThings
{
    /// <summary>
    /// Scans SmartThings for devices whose name/label contains "XT" and that
    /// expose a configured set of attributes. Uses the SmartThings CLI (shell) instead of direct REST.
    /// </summary>
    public sealed class SmartThingsAccess
    {
        // ===== Public model returned by Fetch() =====
        public sealed class ScannedDevice
        {
            public string DeviceId { get; set; }
            public string Name { get; set; }
            public string Label { get; set; }
            public string LocationId { get; set; }
            public string RoomId { get; set; }
            public string HubId { get; set; }
            /// <summary>
            /// Attribute values for the requested attributes, keyed by normalized path
            /// like "main.temperatureMeasurement.temperature".
            /// </summary>
            public Dictionary<string, JToken> Attributes { get; set; } =
                new Dictionary<string, JToken>(StringComparer.OrdinalIgnoreCase);
        }

        // ===== Config / state =====
        private string _locationId;         // optional; auto-discovered if null/empty
        private string _hubId;              // optional
        private TimeSpan _timeout = TimeSpan.FromSeconds(120);
        private List<AttrSpec> _requiredAttrs = new List<AttrSpec>(); // parsed at Init
        private SmartThingsCliRunner _cli;  // CLI runner

        // Attribute spec parsed from strings like "cap.attr" or "component.cap.attr"
        private sealed class AttrSpec
        {
            public string Component { get; set; } = "main";
            public string Capability { get; set; }
            public string Attribute { get; set; }
            public string NormalizedKey => $"{Component}.{Capability}.{Attribute}";
        }

        /// <summary>
        /// Initialize the scanner to use the SmartThings CLI.
        /// - cliPath: path to 'smartthings' executable (or just "smartthings" if in PATH).
        /// - profile: optional CLI profile name (set via SMARTTHINGS_PROFILE env var).
        /// - requiredAttributes: "cap.attr" or "component.cap.attr" (e.g., "temperatureMeasurement.temperature" or "main.switch.switch").
        /// </summary>
        public void InitCli(
            string cliPath,
            IEnumerable<string> requiredAttributes,
            string locationId = null,
            string hubId = null,
            int httpTimeoutSeconds = 120,
            string profile = null)
        {
            if (string.IsNullOrWhiteSpace(cliPath))
                throw new ArgumentException("Path to SmartThings CLI is required (e.g., 'smartthings').", nameof(cliPath));

            _locationId = string.IsNullOrWhiteSpace(locationId) ? null : locationId.Trim();
            _hubId = string.IsNullOrWhiteSpace(hubId) ? null : hubId.Trim();
            _timeout = TimeSpan.FromSeconds(Math.Max(5, httpTimeoutSeconds));

            _requiredAttrs = (requiredAttributes ?? Enumerable.Empty<string>())
                .Select(ParseAttrSpec)
                .ToList();

            if (_requiredAttrs.Count == 0)
                throw new ArgumentException("At least one required attribute is needed.", nameof(requiredAttributes));

            _cli = new SmartThingsCliRunner(cliPath, profile, _timeout);
        }

        /// <summary>
        /// Fetch devices that:
        /// 1) Have any of the specified match terms in name/label, and
        /// 2) Expose at least ONE of the required attributes (from InitCli) in their status.
        /// Returns attribute values for only the requested attributes.
        /// </summary>
        /// 
        // === REPLACE your existing Fetch(...) with THIS ===
        public async Task<List<ScannedDevice>> Fetch(params string[] matchTerms)
        {
            EnsureInitialized();

            if (matchTerms == null || matchTerms.Length == 0)
                throw new ArgumentException("At least one match term is required.", nameof(matchTerms));

            // Normalize match terms to uppercase tokens; ignore blanks
            var tags = new HashSet<string>(
                matchTerms.Where(t => !string.IsNullOrWhiteSpace(t))
                          .Select(t => t.Trim().ToUpperInvariant())
            );

            if (tags.Count == 0)
                throw new ArgumentException("All match terms were empty/whitespace.", nameof(matchTerms));

            // Auto-discover location if not provided
            if (string.IsNullOrWhiteSpace(_locationId))
            {
                var locs = await ListLocationsAsync();
                if (locs.Count == 0)
                    throw new InvalidOperationException("No SmartThings locations found for this CLI profile.");
                _locationId = locs[0].LocationId;
            }

            // Get devices (optionally filtered by hub)
            var all = await ListDevicesAsync(_locationId);
            if (!string.IsNullOrWhiteSpace(_hubId))
                all = all.Where(d => string.Equals(d.HubId, _hubId, StringComparison.OrdinalIgnoreCase)).ToList();

            // Filter to devices where NAME or LABEL contains a tag as a whole token
            var filtered = new List<StDevice>();
            foreach (var d in all)
            {
                var name = d.Name ?? string.Empty;
                var label = d.Label ?? string.Empty;

                if (HasTagToken(name, tags) || HasTagToken(label, tags))
                {
                    filtered.Add(d);
                }
                // else optionally debug:
                // else Log2.Trace($"Skip [{d.DeviceId}] Label='{d.Label}' Name='{d.Name}'");
            }

            var results = new List<ScannedDevice>();

            // Pull status and include only devices with ANY required attribute present
            foreach (var d in filtered)
            {
                var status = await GetDeviceStatusAsync(d.DeviceId);
                var attrMap = TryExtractAnyRequiredAttributes(status, _requiredAttrs);

                if (attrMap != null)
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

        // === ADD this helper inside the class (e.g., under // ===== Helpers =====) ===
        private static bool HasTagToken(string s, HashSet<string> tags)
        {
            if (string.IsNullOrEmpty(s) || tags == null || tags.Count == 0) return false;

            // Build uppercase alphanumeric tokens; any non-alnum char is a delimiter
            var token = new System.Text.StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (char.IsLetterOrDigit(c))
                {
                    token.Append(char.ToUpperInvariant(c));
                }
                else
                {
                    if (token.Length > 0)
                    {
                        if (tags.Contains(token.ToString())) return true;
                        token.Length = 0;
                    }
                }
            }
            // Check trailing token
            if (token.Length > 0 && tags.Contains(token.ToString())) return true;

            return false;
        }

        //public async Task<List<ScannedDevice>> Fetch(params string[] matchTerms)
        //{
        //    EnsureInitialized();

        //    if (matchTerms == null || matchTerms.Length == 0)
        //        throw new ArgumentException("At least one match term is required.", nameof(matchTerms));

        //    // Auto-discover location if not provided
        //    if (string.IsNullOrWhiteSpace(_locationId))
        //    {
        //        var locs = await ListLocationsAsync();
        //        if (locs.Count == 0)
        //            throw new InvalidOperationException("No SmartThings locations found for this CLI profile.");
        //        _locationId = locs[0].LocationId;
        //    }

        //    // Get devices (optionally filtered by hub)
        //    var all = await ListDevicesAsync(_locationId);
        //    if (!string.IsNullOrWhiteSpace(_hubId))
        //        all = all.Where(d => string.Equals(d.HubId, _hubId, StringComparison.OrdinalIgnoreCase)).ToList();

        //    // Filter to devices with any match term
        //    var filtered = all.Where(d =>
        //    {
        //        var name = d.Name ?? "";
        //        var label = d.Label ?? "";
        //        return matchTerms.Any(term =>
        //            (!string.IsNullOrEmpty(name) && name.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0) ||
        //            (!string.IsNullOrEmpty(label) && label.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0));
        //    }).ToList();

        //    var results = new List<ScannedDevice>();

        //    // Pull status and include only devices with ANY required attribute present
        //    foreach (var d in filtered)
        //    {
        //        var status = await GetDeviceStatusAsync(d.DeviceId);
        //        var attrMap = TryExtractAnyRequiredAttributes(status, _requiredAttrs);

        //        if (attrMap != null)
        //        {
        //            results.Add(new ScannedDevice
        //            {
        //                DeviceId = d.DeviceId,
        //                Name = d.Name,
        //                Label = d.Label,
        //                LocationId = d.LocationId,
        //                RoomId = d.RoomId,
        //                HubId = d.HubId,
        //                Attributes = attrMap
        //            });
        //        }
        //    }

        //    return results;
        //}

       

        ///// <summary>
        ///// Fetch devices that:
        ///// 1) Have "XT" in name/label, and
        ///// 2) Expose at least ONE of the required attributes (from InitCli) in their status.
        ///// Returns attribute values for only the requested attributes.
        ///// </summary>
        //public async Task<List<ScannedDevice>> Fetch()
        //{
        //    EnsureInitialized();

        //    // Auto-discover location if not provided
        //    if (string.IsNullOrWhiteSpace(_locationId))
        //    {
        //        var locs = await ListLocationsAsync();
        //        if (locs.Count == 0)
        //            throw new InvalidOperationException("No SmartThings locations found for this CLI profile.");
        //        _locationId = locs[0].LocationId;
        //    }

        //    // Get devices (optionally filtered by hub)
        //    var all = await ListDevicesAsync(_locationId);
        //    if (!string.IsNullOrWhiteSpace(_hubId))
        //        all = all.Where(d => string.Equals(d.HubId, _hubId, StringComparison.OrdinalIgnoreCase)).ToList();

        //    // Filter to XT devices
        //    var xdata = all.Where(d =>
        //    {
        //        var label = d.Label ?? d.Name ?? "";
        //        return label.IndexOf("XT", StringComparison.OrdinalIgnoreCase) >= 0;
        //    }).ToList();

        //    var results = new List<ScannedDevice>();

        //    // Pull status and include only devices with ANY required attribute present
        //    foreach (var d in xdata)
        //    {
        //        var status = await GetDeviceStatusAsync(d.DeviceId);
        //        var attrMap = TryExtractAnyRequiredAttributes(status, _requiredAttrs);

        //        if (attrMap != null)
        //        {
        //            results.Add(new ScannedDevice
        //            {
        //                DeviceId = d.DeviceId,
        //                Name = d.Name,
        //                Label = d.Label,
        //                LocationId = d.LocationId,
        //                RoomId = d.RoomId,
        //                HubId = d.HubId,
        //                Attributes = attrMap
        //            });
        //        }
        //    }

        //    return results;
        //}

        // ===== Helpers =====

        private void EnsureInitialized()
        {
            if (_cli == null)
                throw new InvalidOperationException("Call InitCli(...) before Fetch().");
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
            // smartthings locations --json
            var json = await _cli.RunAsync("locations --json");
            var tok = JToken.Parse(json);

            JArray arr = tok as JArray;
            if (arr == null && tok is JObject jo && jo["items"] is JArray items) arr = items;

            if (arr == null)
                throw new InvalidOperationException("CLI locations JSON not in expected shape:\n" + tok.ToString());

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

        private async Task<List<StDevice>> ListDevicesAsync(string locationId)
        {
            // smartthings devices --json [--location-id <id>]
            var args = string.IsNullOrWhiteSpace(locationId)
                ? "devices --json"
                //: $"devices --location-id {EscapeArg(locationId)} --json";
                : $"devices --json";

            var json = await _cli.RunAsync(args);
            var tok = JToken.Parse(json);

            JArray arr = tok as JArray;
            if (arr == null && tok is JObject jo && jo["items"] is JArray items) arr = items;

            if (arr == null)
                throw new InvalidOperationException("CLI devices JSON not in expected shape:\n" + tok.ToString());

            var list = new List<StDevice>();
            foreach (var el in arr)
            {
                list.Add(new StDevice
                {
                    DeviceId = (string)el["deviceId"],
                    Name = (string)el["name"],
                    Label = (string)el["label"],
                    LocationId = (string)el["locationId"],
                    RoomId = (string)el["roomId"],
                    HubId = (string)el["hubId"]
                });
            }
            return list;
        }

        private async Task<JObject> GetDeviceStatusAsync(string deviceId)
        {
            // smartthings devices:status <deviceId> --json
            var json = await _cli.RunAsync($"devices:status {EscapeArg(deviceId)} --json");

            // The CLI returns a JSON object with "components" similar to REST /status
            var tok = JToken.Parse(json);
            if (tok is JObject o) return o;

            // Fallback: some CLI versions might wrap—handle gracefully
            if (tok is JArray arr && arr.Count > 0 && arr[0] is JObject o2) return o2;

            // Treat as empty if unexpected
            return new JObject { ["components"] = new JObject() };
        }

        private static string EscapeArg(string s)
        {
            if (string.IsNullOrEmpty(s)) return "\"\"";
            if (s.IndexOfAny(new[] { ' ', '\t', '"', '\'' }) >= 0) return "\"" + s.Replace("\"", "\\\"") + "\"";
            return s;
        }

        private static Dictionary<string, JToken> TryExtractAnyRequiredAttributes(JObject statusRoot, List<AttrSpec> required)
        {
            var components = statusRoot["components"] as JObject;
            if (components == null) return null;

            var result = new Dictionary<string, JToken>(StringComparer.OrdinalIgnoreCase);
            foreach (var spec in required)
            {
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

            return result.Count > 0 ? result : null;
        }


      
        // ===== Internal DTOs =====

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

        // ===== CLI runner (process wrapper) =====

        private sealed class SmartThingsCliRunner
        {
            private readonly string _exeOrCmd;
            private readonly string _profile;   // used via SMARTTHINGS_PROFILE env var
            private readonly TimeSpan _timeout;
            private readonly bool _isCmdShim;

            public SmartThingsCliRunner(string exePath, string profile, TimeSpan timeout)
            {
                _profile = string.IsNullOrWhiteSpace(profile) ? null : profile.Trim();
                _timeout = timeout;

                // Normalize input (strip quotes if caller passed them)
                exePath = (exePath ?? "smartthings").Trim().Trim('"');

                _exeOrCmd = ResolveCliPath(exePath, out _isCmdShim);
            }

            public async Task<string> RunAsync(string args)
            {
                // Build actual command to execute
                string fileName, finalArgs;

                if (_isCmdShim)
                {
                    // npm shim: smartthings.cmd — invoke via cmd.exe /c
                    fileName = "cmd.exe";
                    finalArgs = "/c \"" + _exeOrCmd + "\" " + args;
                }
                else
                {
                    fileName = _exeOrCmd;
                    finalArgs = args;
                }

                var psi = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = finalArgs,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8,
                };

                // Set profile via environment (portable across CLI builds)
                if (!string.IsNullOrWhiteSpace(_profile))
                    psi.EnvironmentVariables["SMARTTHINGS_PROFILE"] = _profile;

                // Optional: disable colors for clean JSON, ensure UTF-8
                psi.EnvironmentVariables["FORCE_COLOR"] = "0";
                psi.EnvironmentVariables["NO_COLOR"] = "1";

                var stdout = new StringBuilder();
                var stderr = new StringBuilder();

                using (var p = new Process { StartInfo = psi, EnableRaisingEvents = true })
                {
                    var tcs = new TaskCompletionSource<int>();
                    p.OutputDataReceived += (s, e) => { if (e.Data != null) stdout.AppendLine(e.Data); };
                    p.ErrorDataReceived += (s, e) => { if (e.Data != null) stderr.AppendLine(e.Data); };
                    p.Exited += (s, e) => tcs.TrySetResult(p.ExitCode);

                    try
                    {
                        if (!p.Start())
                            throw new InvalidOperationException("Failed to start SmartThings CLI process.");
                    }
                    catch (System.ComponentModel.Win32Exception win32)
                    {
                        throw new InvalidOperationException(
                            $"Could not start '{fileName}'. Ensure SmartThings CLI is installed and path is correct.\n" +
                            $"ResolvedPath='{_exeOrCmd}', IsCmdShim={_isCmdShim}\n" +
                            $"Original error: {win32.Message}");
                    }

                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();

                    var completed = await Task.WhenAny(tcs.Task, Task.Delay(_timeout)).ConfigureAwait(false);
                    if (completed != tcs.Task)
                    {
                        try { if (!p.HasExited) p.Kill(); } catch { }
                        throw new TimeoutException($"SmartThings CLI timed out after {_timeout.TotalSeconds:N0}s.\nARGS: {finalArgs}");
                    }

                    var exit = tcs.Task.Result;
                    var so = stdout.ToString();
                    var se = stderr.ToString();

                    if (exit != 0)
                        throw new InvalidOperationException(
                            $"smartthings exited with {exit}\nARGS: {finalArgs}\nSTDERR:\n{se}\nSTDOUT:\n{so}");

                    var output = so.Trim();
                    if (string.IsNullOrWhiteSpace(output))
                        throw new InvalidOperationException(
                            $"smartthings produced no JSON output.\nARGS: {finalArgs}\nSTDERR:\n{se}");

                    return output;
                }
            }

            private static string ResolveCliPath(string exePath, out bool isCmdShim)
            {
                isCmdShim = false;

                // If caller passed an absolute path that exists, use it
                if (File.Exists(exePath))
                {
                    isCmdShim = exePath.EndsWith(".cmd", StringComparison.OrdinalIgnoreCase) ||
                                exePath.EndsWith(".bat", StringComparison.OrdinalIgnoreCase);
                    return exePath;
                }

                // Try PATH via 'where smartthings'
                try
                {
                    var wherePsi = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c where smartthings",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    using (var p = Process.Start(wherePsi))
                    {
                        var output = p.StandardOutput.ReadToEnd().Trim();
                        p.WaitForExit(1500);
                        var first = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                        if (!string.IsNullOrEmpty(first) && File.Exists(first))
                        {
                            isCmdShim = first.EndsWith(".cmd", StringComparison.OrdinalIgnoreCase) ||
                                        first.EndsWith(".bat", StringComparison.OrdinalIgnoreCase);
                            return first;
                        }
                    }
                }
                catch { /* ignore */ }

                // Common install locations
                var guesses = new[]
                {
                    // MSI defaults
                    @"C:\Program Files\SmartThings\smartthings.exe",
                    @"C:\Program Files (x86)\SmartThings\smartthings.exe",
                    // npm globals
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"npm\smartthings.cmd"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"npm\smartthings.exe"),
                };

                foreach (var g in guesses)
                {
                    if (File.Exists(g))
                    {
                        isCmdShim = g.EndsWith(".cmd", StringComparison.OrdinalIgnoreCase) ||
                                    g.EndsWith(".bat", StringComparison.OrdinalIgnoreCase);
                        return g;
                    }
                }

                // Last resort: return as-is (CreateProcess will fail with a clear message)
                return exePath;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upperbay.Worker.SmartThings;
using Upperbay.Core.Library;
using Upperbay.Core.Logging;
using Upperbay.Worker.EtherAccess;
using System.Data.SqlTypes;
using System.Configuration;
using System.Runtime.InteropServices;


namespace SmartThingsRunner
{
    internal class Program
    {
        static string StripOuterQuotes(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            s = s.Trim();
            if (s.Length >= 2 &&
                ((s.StartsWith("\"") && s.EndsWith("\"")) || (s.StartsWith("'") && s.EndsWith("'"))))
            {
                return s.Substring(1, s.Length - 2);
            }
            return s;
        }

        public static string QuoteForFetch(string term)
        {
            if (term is null) return string.Empty;

            // Remove existing surrounding quotes, keep the inner content
            var raw = StripOuterQuotes(term.Trim());

            // Escape backslash + quotes to cooperate with your TokenizeWithQuotes()
            var escaped = raw
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("'", "\\'");

            bool needsQuotes = escaped.Any(char.IsWhiteSpace) || escaped.Contains("\"") || escaped.Contains("'");

            return needsQuotes ? $"\"{escaped}\"" : escaped;
        }

        public static string[] BuildQuotedTags(params string[] terms)
        {
            return (terms ?? Array.Empty<string>())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(QuoteForFetch)
                .ToArray();
        }

        // Optional: single CLI-style line (space-delimited)
        public static string BuildQuotedArgLine(params string[] terms)
        {
            return string.Join(" ", BuildQuotedTags(terms));
        }

        public abstract class NamedAttribute
        {
            public string Name { get; }
            public string Attribute { get; }

            protected NamedAttribute(string name, string attribute)
            {
                if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required.", nameof(name));
                if (string.IsNullOrWhiteSpace(attribute)) throw new ArgumentException("Attribute required.", nameof(attribute));
                Name = name;
                Attribute = attribute;
            }

            public override string ToString() => $"{Name} [{Attribute}]";
        }

        // Example concrete type
        public sealed class DeviceTag : NamedAttribute
        {
            public DeviceTag(string name, string attribute) : base(name, attribute) { }
        }


        static void Main(string[] args)
        {
            
            try
            {
               
                var now = DateTime.Now;

                Console.WriteLine("START SCANNING SMARTTHINGS");
                List<DeviceTag> deviceTags = new List<DeviceTag>();
                DeviceTag deviceTag = new DeviceTag("3 SECOND LOC SOIL IT", "relativeHumidityMeasurement.humidity");
                deviceTags.Add(deviceTag);
                DeviceTag deviceTag1 = new DeviceTag("3 Drain 2 XT", "relativeHumidityMeasurement.humidity");
                deviceTags.Add(deviceTag1);
                DeviceTag deviceTag2 = new DeviceTag("Light Sensor", "illuminanceMeasurement.illuminance");
                deviceTags.Add(deviceTag2);


                

                List<string> deviceAttributes = deviceTags
                    .Select(t => t.Attribute)
                    .Where(a => !string.IsNullOrWhiteSpace(a))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                Console.WriteLine(string.Join(", ", deviceAttributes));

                //var scanner = new SmartThingsAccess();
                var st = new Upperbay.Worker.SmartThings.SmartThingsAccessExt();
                // Require switch state and temperature (from main component)

                st.InitCli(
                    cliPath: @"C:\Program Files (x86)\SmartThings\smartthings.exe",
                    requiredAttributes: deviceAttributes,   // IEnumerable<string>
                    locationId: null,
                    hubId: null,
                    httpTimeoutSeconds: 120,
                    profile: "default"
                );




                //st.InitCli(
                //    cliPath: "C:\\Program Files (x86)\\SmartThings\\smartthings.exe", // or full path to smartthings.exe
                //    requiredAttributes: argstring,
                //    locationId: null,        // auto-discover first location
                //    hubId: null,
                //    httpTimeoutSeconds: 120,
                //    profile: "default"                           // optional CLI profile name
                //);

                //st.InitCli(
                //    cliPath: "C:\\Program Files (x86)\\SmartThings\\smartthings.exe", // or full path to smartthings.exe
                //    requiredAttributes: new[] {
                //            "relativeHumidityMeasurement.humidity",
                //            "temperatureMeasurement.temperature"
                //    },
                //    locationId: null,        // auto-discover first location
                //    hubId: null,
                //    httpTimeoutSeconds: 120,
                //    profile: "default"                           // optional CLI profile name
                //);
                // Find devices with XT or IT in their name/label



                List<string> deviceNames = new List<string>();
                foreach (DeviceTag tag in deviceTags)
                {
                    deviceNames.Add(tag.Name);
                }

                var argLine = BuildQuotedArgLine(deviceNames.ToArray());
                Console.WriteLine(argLine);

                var devices = st.Fetch(argLine).GetAwaiter().GetResult();

                //var devices = st.Fetch().GetAwaiter().GetResult();

                // Ensure logs directory exists
                string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
                Directory.CreateDirectory(logDir);

                // Use a date-based CSV log file (one per day)
                //string csvPath = Path.Combine(logDir, $"RainMan_Devices_{DateTime.Now:yyyy-MM-dd}.csv");
                string csvPath = Path.Combine(logDir, $"DataMan_Devices.csv");

                // Add header if file is new
                if (!File.Exists(csvPath))
                {
                    File.AppendAllText(csvPath, "Timestamp,DeviceName,Data1, Data2" + Environment.NewLine);
                }

                foreach (var d in devices)
                {
                    Console.WriteLine($"{d.Label ?? d.Name} [{d.DeviceId}]");

                    // Safely get temperature & humidity
                    //string temp = d.Attributes.TryGetValue("main.temperatureMeasurement.temperature", out var t) ? t?.ToString() ?? "na" : "na";
                    //string hum = d.Attributes.TryGetValue("main.relativeHumidityMeasurement.humidity", out var h) ? h?.ToString() ?? "na" : "na";

                    //// Debug log
                    //Console.WriteLine($"  temperature = {temp}");
                    //Console.WriteLine($"  humidity = {hum}");

                    // Build CSV-safe line
                    //string csvLine = string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"",
                    //    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    //    (d.Label ?? d.Name).Replace("\"", "\"\""),
                    //    temp.Replace("\"", "\"\""),
                    //    hum.Replace("\"", "\"\""));

                    //File.AppendAllText(csvPath, csvLine + Environment.NewLine);

                    foreach (var kvp in d.Attributes)
                    {
                        Console.WriteLine($"  {kvp.Key} = {kvp.Value}");

                        // Build CSV-safe line
                        string csvLine1 = string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            (d.Label ?? d.Name).Replace("\"", "\"\""),
                            //d.DeviceId,
                            kvp.Key.Replace("\"", "\"\""),
                            kvp.Value != null ? kvp.Value.ToString().Replace("\"", "\"\"") : "");

                        File.AppendAllText(csvPath, csvLine1 + Environment.NewLine);
                    }
                }

                //foreach (var d in devices)
                //{
                //    Log2.Debug($"{d.Label ?? d.Name} [{d.DeviceId}]");
                //    string name = name = (d.Label ?? d.Name).Replace("\"", "\"\"");
                //    if (name.Contains("XT"))
                //    {
                //        // Safely get temperature & humidity
                //        string temp = d.Attributes.TryGetValue("main.temperatureMeasurement.temperature", out var t) ? t?.ToString() ?? "na" : "na";
                //        string hum = d.Attributes.TryGetValue("main.relativeHumidityMeasurement.humidity", out var h) ? h?.ToString() ?? "na" : "na";

                //        // Debug log
                //        Log2.Debug($"  temperature = {temp}");
                //        Log2.Debug($"  humidity = {hum}");

                //        string rainManChainEnabled = MyAppConfig.GetParameter("RainManChainEnabled");
                //        if (rainManChainEnabled == "true")
                //        {
                //            string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //            string zip = rainManZipcode;
                //            string com = rainManCommunity;
                //            string lot = rainManLotNumber;
                //            //string name = (d.Label ?? d.Name).Replace("\"", "\"\"");
                //            string temperature = temp.Replace("\"", "\"\"");
                //            string humidity = hum.Replace("\"", "\"\"");
                //            Log2.Debug("Calling RainLedger.AddSoilRecord: {0}", name);

                //            RainLedger rainLedger = new RainLedger();
                //            var task = rainLedger.AddSoilRecord(dt,
                //                                    zip,
                //                                    com,
                //                                    lot,
                //                                    name,
                //                                    temperature,
                //                                    humidity);
                //        }
                //    }
                //}
                Console.WriteLine("END SCANNING SMARTTHINGS");
            //_rainEnabled = false;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }



     

        public class RunOnceHelper
        {
            private readonly object _gate = new object();
            private readonly Dictionary<string, DateTime> _lastBucketRun = new Dictionary<string, DateTime>();
            private readonly HashSet<string> _virginSchedules = new HashSet<string>();

            public RunOnceHelper()
            {
            }

            /// <summary>
            /// Returns true exactly once at the start of each cadence bucket (e.g., every 5 minutes,
            /// or every 6 hours), allowing a ±toleranceSeconds window around the boundary.
            /// - scheduleName: unique name for this schedule (e.g., "every-5-min", "six-hour").
            /// - now: current time (use DateTime.Now or DateTime.UtcNow consistently).
            /// - cadence: e.g., TimeSpan.FromMinutes(5) or TimeSpan.FromHours(6).
            /// - toleranceSeconds: window around the boundary (e.g., 10).
            /// - fireVirginImmediately: if true, fires once immediately on first call for each scheduleName.
            /// </summary>
            public bool ShouldRunOnceAtBoundary(
                string scheduleName,
                DateTime now,
                TimeSpan cadence,
                int toleranceSeconds,
                bool fireVirginImmediately = true)
            {
                if (string.IsNullOrEmpty(scheduleName))
                    throw new ArgumentException("Schedule name is required.", "scheduleName");

                if (cadence <= TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException("cadence", "Cadence must be positive.");

                // Snap 'now' down to the start of its cadence bucket: floor(now, cadence)
                long ticksPerBucket = cadence.Ticks;
                long flooredTicks = (now.Ticks / ticksPerBucket) * ticksPerBucket;
                var bucketStart = new DateTime(flooredTicks, now.Kind);

                // Only fire near the bucket boundary
                double secondsFromBoundary = Math.Abs((now - bucketStart).TotalSeconds);
                bool withinTolerance = secondsFromBoundary <= toleranceSeconds;

                if (!withinTolerance)
                    return false;

                lock (_gate)
                {
                    // First run for this schedule?
                    if (fireVirginImmediately && !_virginSchedules.Contains(scheduleName))
                    {
                        _virginSchedules.Add(scheduleName);
                        _lastBucketRun[scheduleName] = bucketStart;
                        return true;
                    }

                    DateTime last;
                    if (_lastBucketRun.TryGetValue(scheduleName, out last))
                    {
                        if (last == bucketStart)
                            return false; // Already fired for this bucket
                    }

                    _lastBucketRun[scheduleName] = bucketStart;
                    return true;
                }
            }
        }
    }
}


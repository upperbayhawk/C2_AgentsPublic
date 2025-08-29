// Copyright (C) Upperbay Systems, LLC - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Dave Hardin <dave@upperbay.com>, 2001-2020

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Upperbay.AgentObject.DataAgent;

public sealed class DataManCsvWriter
{
    private readonly string _csvPath;
    private readonly List<DeviceTag> _deviceTags;
    private readonly List<string> _columns; // device names (from tags), in a stable order

    public DataManCsvWriter(string csvPath, List<DeviceTag> deviceTags)
    {
        if (string.IsNullOrWhiteSpace(csvPath)) throw new ArgumentException("csvPath required", "csvPath");
        if (deviceTags == null || deviceTags.Count == 0) throw new ArgumentException("deviceTags required", "deviceTags");

        _csvPath = csvPath;
        _deviceTags = deviceTags;

        // Header columns = distinct tag names (case-insensitive)
        _columns = _deviceTags
            .Select(t => t.Name)
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        WriteHeaderIfNeeded();
    }

    public void AppendRow(IList<ScannedDevice> devices)
    {
        if (devices == null) throw new ArgumentNullException("devices");

        // Defaults = "na"
        var row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var col in _columns) row[col] = "na";

        // For each device, for each tag that matches by Name or Label, write the attribute value
        foreach (var d in devices)
        {
            string deviceDisplay = string.IsNullOrEmpty(d.Label) ? (d.Name ?? "") : d.Label;

            foreach (var tag in _deviceTags)
            {
                bool isMatch =
                    string.Equals(d.Name ?? "", tag.Name ?? "", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(deviceDisplay, tag.Name ?? "", StringComparison.OrdinalIgnoreCase);

                if (!isMatch) continue;

                string value = "na";
                if (d.Attributes != null)
                {
                    object tmp;
                    if (d.Attributes.TryGetValue(tag.Attribute, out tmp) && tmp != null)
                        value = tmp.ToString();
                }

                row[tag.Name] = value; // write to the column named by the tag
            }
        }

        // Build CSV line: Timestamp + values in header order
        var parts = new List<string>(_columns.Count + 1);
        parts.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        foreach (var col in _columns) parts.Add(EscapeCsv(row[col]));

        File.AppendAllText(_csvPath, string.Join(",", parts.ToArray()) + Environment.NewLine);
    }

    private void WriteHeaderIfNeeded()
    {
        bool needHeader = !File.Exists(_csvPath) || new FileInfo(_csvPath).Length == 0;
        if (!needHeader) return;

        var header = new List<string>(_columns.Count + 1);
        header.Add("Timestamp");
        foreach (var c in _columns) header.Add(EscapeCsv(c));

        File.AppendAllText(_csvPath, string.Join(",", header.ToArray()) + Environment.NewLine);
    }

    private static string EscapeCsv(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        if (s.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0)
            return "\"" + s.Replace("\"", "\"\"") + "\"";
        return s;
    }
}

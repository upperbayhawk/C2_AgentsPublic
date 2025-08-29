using System.Text;
using System.Text.RegularExpressions;

class Program
{
    // Folders to skip
    static readonly string[] SkipDirs = { ".git", "bin", "obj", "node_modules", ".vs", ".idea", ".gitlab", "packages" };
    const long MaxFileBytes = 2_000_000; // 2 MB cap
    const string InlineAllow = "secret-scan: ignore-line";

    // Known secret patterns (extend as needed)
    static readonly (string Name, Regex Rx)[] Patterns = new (string, Regex)[] {
        ("OpenAI", new Regex(@"sk-[A-Za-z0-9]{32,48}", RegexOptions.Compiled)),
        ("AWS Access Key ID", new Regex(@"\bAKIA[0-9A-Z]{16}\b", RegexOptions.Compiled)),
        ("AWS Secret", new Regex(@"(?i)aws(.{0,20})?(secret|access)?(.{0,20})?(key|token)\s*[:=]\s*[A-Za-z0-9/+=]{40}\b", RegexOptions.Compiled)),
        ("GitHub Token", new Regex(@"\bghp_[A-Za-z0-9]{36}\b|\bgithub_pat_[A-Za-z0-9_]{20,}\b", RegexOptions.Compiled)),
        ("Stripe Secret", new Regex(@"\bsk_(live|test)_[A-Za-z0-9]{24,}\b", RegexOptions.Compiled)),
        ("Google API Key", new Regex(@"\bAIza[0-9A-Za-z\-_]{35}\b", RegexOptions.Compiled)),
        ("Slack Token", new Regex(@"\bxox[baprs]-[A-Za-z0-9-]{10,48}\b", RegexOptions.Compiled)),
        ("Azure Storage Key", new Regex(@"AccountKey=[A-Za-z0-9+/=]{80,}", RegexOptions.Compiled)),
        ("JWT", new Regex(@"\beyJ[A-Za-z0-9_\-]{10,}\.[A-Za-z0-9_\-]{10,}\.[A-Za-z0-9_\-]{10,}\b", RegexOptions.Compiled)),
        ("PEM Private Key", new Regex(@"-----BEGIN (?:RSA|EC|DSA|OPENSSH|PRIVATE) KEY-----", RegexOptions.Compiled)),
        ("Ethereum Private Key", new Regex(@"\b0x?[0-9a-fA-F]{64}\b", RegexOptions.Compiled)),
    };

    // Entropy candidate patterns
    static readonly Regex Base64Blob = new(@"[A-Za-z0-9+/]{32,}={0,2}", RegexOptions.Compiled);
    static readonly Regex HexBlob = new(@"\b[0-9A-Fa-f]{48,}\b", RegexOptions.Compiled);

    record Finding(string File, int Line, string Rule, string Snippet);

    static int Main(string[] args)
    {
        var root = args.Length > 0 ? args[0] : Directory.GetCurrentDirectory();
        if (!Directory.Exists(root)) { Console.Error.WriteLine($"Path not found: {root}"); return 2; }

        var findings = new List<Finding>();

        foreach (var file in EnumerateTextFiles(root))
        {
            try
            {
                var lines = File.ReadAllLines(file, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false));
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (line.Contains(InlineAllow, StringComparison.OrdinalIgnoreCase)) continue;

                    // Known patterns
                    foreach (var (name, rx) in Patterns)
                    {
                        foreach (Match m in rx.Matches(line))
                        {
                            findings.Add(new Finding(file, i + 1, name, Redact(m.Value)));
                        }
                    }

                    // Entropy-based generic detection (base64 / hex)
                    foreach (Match m in Base64Blob.Matches(line))
                    {
                        if (Shannon(m.Value) >= 4.0) findings.Add(new Finding(file, i + 1, "High-entropy (base64)", Redact(m.Value)));
                    }
                    foreach (Match m in HexBlob.Matches(line))
                    {
                        if (Shannon(m.Value) >= 3.5) findings.Add(new Finding(file, i + 1, "High-entropy (hex)", Redact(m.Value)));
                    }
                }
            }
            catch
            {
                // Skip unreadable files quietly
            }
        }

        if (findings.Count == 0)
        {
            Console.WriteLine("SecretScan: ✅ No potential secrets found.");
            return 0;
        }

        Console.WriteLine("SecretScan: ❌ Potential secrets detected:");
        foreach (var f in findings.OrderBy(f => f.File).ThenBy(f => f.Line))
        {
            Console.WriteLine($"  [{f.Rule}] {f.File}:{f.Line}  =>  {f.Snippet}");
        }

        Console.WriteLine("\nTip: add 'secret-scan: ignore-line' to suppress a false positive on a specific line.");
        return 1; // fail
    }

    static IEnumerable<string> EnumerateTextFiles(string root)
    {
        var stack = new Stack<string>();
        stack.Push(root);

        while (stack.Count > 0)
        {
            var dir = stack.Pop();
            string name = Path.GetFileName(dir);
            if (SkipDirs.Contains(name, StringComparer.OrdinalIgnoreCase)) continue;

            IEnumerable<string> subdirs = Enumerable.Empty<string>();
            IEnumerable<string> files = Enumerable.Empty<string>();
            try
            {
                subdirs = Directory.EnumerateDirectories(dir);
                files = Directory.EnumerateFiles(dir);
            }
            catch
            {
                // Skip directories we can't read
            }

            foreach (var sd in subdirs)
                stack.Push(sd);

            foreach (var f in files)
            {
                bool include = false;
                try
                {
                    var fi = new FileInfo(f);
                    // decide eligibility inside try...
                    include = fi.Length <= MaxFileBytes && !LooksBinary(f);
                }
                catch
                {
                    include = false; // unreadable file -> skip
                }

                // ...and do the yield OUTSIDE the try/catch
                if (include)
                    yield return f;
            }
        }
    }


    static bool LooksBinary(string file)
    {
        try
        {
            var bytes = File.ReadAllBytes(file);
            if (bytes.Length == 0) return false;
            int nonText = 0; int check = Math.Min(bytes.Length, 4096);
            for (int i = 0; i < check; i++)
            {
                byte b = bytes[i];
                // common text bytes: tab, newline, carriage return, form feed, standard printable range
                if (b == 9 || b == 10 || b == 13 || (b >= 32 && b < 127)) continue;
                nonText++;
            }
            return nonText > check * 0.2; // >20% non-text → treat as binary
        }
        catch { return true; }
    }

    static double Shannon(string s)
    {
        if (string.IsNullOrEmpty(s)) return 0;
        var counts = new Dictionary<char, int>();
        foreach (var c in s) counts[c] = counts.TryGetValue(c, out var v) ? v + 1 : 1;
        double H = 0.0;
        foreach (var kv in counts)
        {
            double p = (double)kv.Value / s.Length;
            H -= p * Math.Log(p, 2);
        }
        return H;
    }

    static string Redact(string value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        if (value.Length <= 6) return new string('*', value.Length);
        return $"{value.Substring(0, 3)}…{value.Substring(value.Length - 3)}";
    }
}

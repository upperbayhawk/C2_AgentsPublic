using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using Newtonsoft.Json;

namespace SmartThingsOAuthHttps
{

    //    Install cloudflared and log in.
    //winget install --id Cloudflare.cloudflared
    //cloudflared tunnel --url https://localhost:5001
    //Use the printed public URL as your SmartThings redirect.
    //No code changes needed—just keep the HttpListener on https://localhost:5001/callback/.
    class Program
    {
        // ====== Fill with your SmartThings app creds ======
        private const string ClientId = "<YOUR_CLIENT_ID>";
        private const string ClientSecret = "<YOUR_CLIENT_SECRET>"; // may be empty for public clients
        private const string RedirectUri = "https://localhost:5001/callback/"; // must match app config
        private const string AuthUrl = "https://api.smartthings.com/oauth/authorize";
        private const string TokenUrl = "https://api.smartthings.com/oauth/token";

        // Request only what you need
        private const string Scopes = "r:devices:* x:devices:*";

        private static readonly string TokenPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tokens.json");

        static void Main()
        {
            try
            {
                RunAsync().GetAwaiter().GetResult();
                Console.WriteLine("\nDone. Press Enter to exit.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fatal: " + ex);
                Console.ResetColor();
            }
            Console.ReadLine();
        }

        static async System.Threading.Tasks.Task RunAsync()
        {
            var store = new FileTokenStore(TokenPath);
            var oauth = new OAuth2Client(ClientId, ClientSecret, AuthUrl, TokenUrl, RedirectUri, store);

            // Start local HTTPS listener
            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add(RedirectUri);
                listener.Start();
                Console.WriteLine("Listening on " + RedirectUri);

                // Build the authorize URL with PKCE
                Pkce.Create(out var verifier, out var challenge);
                var authorizeUrl = oauth.BuildAuthorizeUrl(Scopes, state: "st_oauth_https", codeChallenge: challenge);
                Console.WriteLine("Opening browser for consent...");
                OpenInBrowser(authorizeUrl);

                // Wait for the first callback
                var ctx = await listener.GetContextAsync().ConfigureAwait(false);
                var req = ctx.Request;

                var code = req.QueryString["code"];
                var state = req.QueryString["state"];

                if (string.IsNullOrWhiteSpace(code))
                    throw new InvalidOperationException("Authorization code not found in callback.");

                // Respond to the browser
                var html = "<html><body><h2>SmartThings OAuth (HTTPS) complete ✔</h2>You can return to the app.</body></html>";
                var bytes = Encoding.UTF8.GetBytes(html);
                ctx.Response.ContentType = "text/html; charset=utf-8";
                ctx.Response.ContentLength64 = bytes.Length;
                using (var os = ctx.Response.OutputStream) os.Write(bytes, 0, bytes.Length);

                // We don't need to keep listening
                listener.Stop();

                Console.WriteLine("Exchanging code for tokens...");
                await oauth.ExchangeCodeAsync(code, codeVerifier: verifier);

                // Use token (auto-refreshes when needed)
                using (var http = new HttpClient())
                {
                    await oauth.AttachBearerAsync(http);
                    var resp = await http.GetAsync("https://api.smartthings.com/v1/devices").ConfigureAwait(false);
                    var body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Console.WriteLine("\nGET /v1/devices -> " + (int)resp.StatusCode);
                    Console.WriteLine(body);
                }

                Console.WriteLine("\nToken file saved at: " + TokenPath);
            }
        }

        private static void OpenInBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch
            {
                Console.WriteLine("Open this URL manually:\n" + url);
            }
        }
    }

    // ===== Minimal OAuth2 client with refresh support =====
    public sealed class OAuth2Client : IDisposable
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly Uri _authorizeEndpoint;
        private readonly Uri _tokenEndpoint;
        private readonly string _redirectUri;
        private readonly ITokenStore _store;
        private readonly TimeSpan _skew = TimeSpan.FromMinutes(2);
        private readonly HttpClient _http;

        public OAuth2Client(string clientId, string clientSecret, string authorizeEndpoint, string tokenEndpoint, string redirectUri, ITokenStore store)
        {
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentException("clientId required");
            if (string.IsNullOrWhiteSpace(authorizeEndpoint)) throw new ArgumentException("authorizeEndpoint required");
            if (string.IsNullOrWhiteSpace(tokenEndpoint)) throw new ArgumentException("tokenEndpoint required");
            if (string.IsNullOrWhiteSpace(redirectUri)) throw new ArgumentException("redirectUri required");

            _clientId = clientId.Trim();
            _clientSecret = (clientSecret ?? "").Trim();
            _authorizeEndpoint = new Uri(authorizeEndpoint);
            _tokenEndpoint = new Uri(tokenEndpoint);
            _redirectUri = redirectUri.Trim();
            _store = store ?? new InMemoryTokenStore();
            _http = new HttpClient { Timeout = TimeSpan.FromSeconds(100) };
        }

        public string BuildAuthorizeUrl(string scope, string state = null, string codeChallenge = null, string codeChallengeMethod = "S256")
        {
            var sb = new StringBuilder();
            sb.Append(_authorizeEndpoint.AbsoluteUri);
            sb.Append(_authorizeEndpoint.AbsoluteUri.Contains("?") ? "&" : "?");
            sb.Append("response_type=code");
            sb.Append("&client_id=").Append(Uri.EscapeDataString(_clientId));
            sb.Append("&redirect_uri=").Append(Uri.EscapeDataString(_redirectUri));
            if (!string.IsNullOrWhiteSpace(scope)) sb.Append("&scope=").Append(Uri.EscapeDataString(scope));
            if (!string.IsNullOrWhiteSpace(state)) sb.Append("&state=").Append(Uri.EscapeDataString(state));
            if (!string.IsNullOrWhiteSpace(codeChallenge))
            {
                sb.Append("&code_challenge=").Append(Uri.EscapeDataString(codeChallenge));
                sb.Append("&code_challenge_method=").Append(Uri.EscapeDataString(codeChallengeMethod));
            }
            return sb.ToString();
        }

        public async System.Threading.Tasks.Task<TokenResponse> ExchangeCodeAsync(string code, string codeVerifier = null, System.Threading.CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("authorization code required");

            // Build form content depending on whether we have client_secret and/or PKCE verifier
            var content = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>
            {
                new System.Collections.Generic.KeyValuePair<string,string>("grant_type","authorization_code"),
                new System.Collections.Generic.KeyValuePair<string,string>("code", code),
                new System.Collections.Generic.KeyValuePair<string,string>("redirect_uri", _redirectUri),
                new System.Collections.Generic.KeyValuePair<string,string>("client_id", _clientId),
            };
            if (!string.IsNullOrEmpty(_clientSecret))
                content.Add(new System.Collections.Generic.KeyValuePair<string, string>("client_secret", _clientSecret));
            if (!string.IsNullOrWhiteSpace(codeVerifier))
                content.Add(new System.Collections.Generic.KeyValuePair<string, string>("code_verifier", codeVerifier));

            using (var resp = await _http.PostAsync(_tokenEndpoint, new FormUrlEncodedContent(content), ct).ConfigureAwait(false))
            {
                var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!resp.IsSuccessStatusCode)
                    throw new HttpRequestException($"Token exchange failed: {(int)resp.StatusCode} {resp.ReasonPhrase}\n{json}");

                var token = JsonConvert.DeserializeObject<TokenResponse>(json) ?? new TokenResponse();
                token.IssuedUtc = DateTimeOffset.UtcNow;
                _store.Save(token);
                return token;
            }
        }

        public async System.Threading.Tasks.Task<string> GetAccessTokenAsync(System.Threading.CancellationToken ct = default)
        {
            var t = _store.Load();
            if (t == null) throw new InvalidOperationException("No tokens stored. Complete the auth flow first.");

            if (IsExpiredOrNear(t))
            {
                if (string.IsNullOrWhiteSpace(t.RefreshToken))
                    throw new InvalidOperationException("Access token expired and no refresh token is available.");
                t = await RefreshAsync(t.RefreshToken, ct).ConfigureAwait(false);
            }
            return t.AccessToken;
        }

        public async System.Threading.Tasks.Task AttachBearerAsync(HttpClient client, System.Threading.CancellationToken ct = default)
        {
            var access = await GetAccessTokenAsync(ct).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access);
        }

        public async System.Threading.Tasks.Task<TokenResponse> RefreshAsync(string refreshToken, System.Threading.CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken)) throw new ArgumentException("refresh token required");

            var content = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>
            {
                new System.Collections.Generic.KeyValuePair<string,string>("grant_type","refresh_token"),
                new System.Collections.Generic.KeyValuePair<string,string>("refresh_token", refreshToken),
                new System.Collections.Generic.KeyValuePair<string,string>("client_id", _clientId),
            };
            if (!string.IsNullOrEmpty(_clientSecret))
                content.Add(new System.Collections.Generic.KeyValuePair<string, string>("client_secret", _clientSecret));

            using (var resp = await _http.PostAsync(_tokenEndpoint, new FormUrlEncodedContent(content), ct).ConfigureAwait(false))
            {
                var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!resp.IsSuccessStatusCode)
                    throw new HttpRequestException($"Token refresh failed: {(int)resp.StatusCode} {resp.ReasonPhrase}\n{json}");

                var newToken = JsonConvert.DeserializeObject<TokenResponse>(json) ?? new TokenResponse();
                newToken.IssuedUtc = DateTimeOffset.UtcNow;

                // If server didn't send a new refresh token, keep the old one
                var old = _store.Load();
                if (string.IsNullOrWhiteSpace(newToken.RefreshToken) && old != null)
                    newToken.RefreshToken = old.RefreshToken;

                _store.Save(newToken);
                return newToken;
            }
        }

        private bool IsExpiredOrNear(TokenResponse t)
        {
            if (string.IsNullOrWhiteSpace(t.AccessToken)) return true;
            var lifetime = t.ExpiresIn > 0 ? TimeSpan.FromSeconds(t.ExpiresIn) : TimeSpan.FromHours(1);
            return DateTimeOffset.UtcNow + TimeSpan.FromMinutes(2) >= t.IssuedUtc + lifetime;
        }

        public void Dispose() { _http.Dispose(); }
    }

    // ===== Token models & stores (sync file I/O) =====

    public sealed class TokenResponse
    {
        [JsonProperty("access_token")] public string AccessToken { get; set; }
        [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
        [JsonProperty("token_type")] public string TokenType { get; set; }
        [JsonProperty("expires_in")] public int ExpiresIn { get; set; }
        [JsonProperty("scope")] public string Scope { get; set; }

        [JsonIgnore] public DateTimeOffset IssuedUtc { get; set; }
    }

    public interface ITokenStore
    {
        void Save(TokenResponse token);
        TokenResponse Load();
    }

    public sealed class InMemoryTokenStore : ITokenStore
    {
        private TokenResponse _t;
        public void Save(TokenResponse token) { _t = token; }
        public TokenResponse Load() => _t;
    }

    public sealed class FileTokenStore : ITokenStore
    {
        private readonly string _path;
        public FileTokenStore(string path) { _path = path; }

        public void Save(TokenResponse token)
        {
            var dir = Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
            var json = JsonConvert.SerializeObject(token, Formatting.Indented);
            File.WriteAllText(_path, json);
        }

        public TokenResponse Load()
        {
            if (!File.Exists(_path)) return null;
            var json = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<TokenResponse>(json);
        }
    }

    // ===== Tiny PKCE helper =====
    public static class Pkce
    {
        public static void Create(out string verifier, out string challenge)
        {
            // High-entropy URL-safe verifier, then SHA-256 to challenge
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            verifier = Base64Url(bytes) + Base64Url(bytes); // ~86 chars

            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var hash = sha.ComputeHash(Encoding.ASCII.GetBytes(verifier));
                challenge = Base64Url(hash);
            }
        }

        private static string Base64Url(byte[] data)
        {
            return Convert.ToBase64String(data).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OAuthSupport
{
    /// <summary>
    /// Minimal OAuth2 client with refresh-token support for .NET Framework 4.8.
    /// Supports Authorization Code (with/without PKCE).
    /// </summary>
    public sealed class OAuth2Client : IDisposable
    {
        private readonly string _clientId;
        private readonly string _clientSecret;       // optional for public clients
        private readonly Uri _authorizeEndpoint;
        private readonly Uri _tokenEndpoint;
        private readonly string _redirectUri;
        private readonly ITokenStore _store;
        private readonly TimeSpan _expirySkew = TimeSpan.FromMinutes(2); // refresh a bit early

        private readonly HttpClient _http;

        public OAuth2Client(
            string clientId,
            string clientSecret,
            string authorizeEndpoint,
            string tokenEndpoint,
            string redirectUri,
            ITokenStore store = null,
            TimeSpan? httpTimeout = null)
        {
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentException("clientId required");
            if (string.IsNullOrWhiteSpace(authorizeEndpoint)) throw new ArgumentException("authorizeEndpoint required");
            if (string.IsNullOrWhiteSpace(tokenEndpoint)) throw new ArgumentException("tokenEndpoint required");
            if (string.IsNullOrWhiteSpace(redirectUri)) throw new ArgumentException("redirectUri required");

            _clientId = clientId;
            _clientSecret = clientSecret ?? "";
            _authorizeEndpoint = new Uri(authorizeEndpoint);
            _tokenEndpoint = new Uri(tokenEndpoint);
            _redirectUri = redirectUri;
            _store = store ?? new InMemoryTokenStore();

            _http = new HttpClient { Timeout = httpTimeout ?? TimeSpan.FromSeconds(100) };
        }

        /// <summary>
        /// Build the user authorization URL. Send the user here to consent/login.
        /// </summary>
        public string BuildAuthorizeUrl(
            string scope,
            string state = null,
            string codeChallenge = null,      // PKCE code_challenge (optional)
            string codeChallengeMethod = "S256")
        {
            var sb = new StringBuilder();
            sb.Append(_authorizeEndpoint.AbsoluteUri);
            sb.Append(_authorizeEndpoint.Query?.Length > 1 ? "&" : (_authorizeEndpoint.AbsoluteUri.Contains("?") ? "&" : "?"));
            sb.Append("response_type=code");
            sb.Append("&client_id=").Append(Uri.EscapeDataString(_clientId));
            sb.Append("&redirect_uri=").Append(Uri.EscapeDataString(_redirectUri));
            if (!string.IsNullOrWhiteSpace(scope))
                sb.Append("&scope=").Append(Uri.EscapeDataString(scope));
            if (!string.IsNullOrWhiteSpace(state))
                sb.Append("&state=").Append(Uri.EscapeDataString(state));
            if (!string.IsNullOrWhiteSpace(codeChallenge))
            {
                sb.Append("&code_challenge=").Append(Uri.EscapeDataString(codeChallenge));
                sb.Append("&code_challenge_method=").Append(Uri.EscapeDataString(codeChallengeMethod));
            }
            return sb.ToString();
        }

        /// <summary>
        /// After the user returns to redirectUri with ?code=..., call this to exchange the code for tokens.
        /// </summary>
        public async Task<TokenResponse> ExchangeCodeAsync(string code, string codeVerifier = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("authorization code required");

            var form = new MultipartFormDataContent
            {
                { new StringContent("authorization_code"), "grant_type" },
                { new StringContent(code), "code" },
                { new StringContent(_redirectUri), "redirect_uri" },
                { new StringContent(_clientId), "client_id" }
            };

            if (!string.IsNullOrEmpty(_clientSecret))
                form.Add(new StringContent(_clientSecret), "client_secret");

            if (!string.IsNullOrWhiteSpace(codeVerifier))
                form.Add(new StringContent(codeVerifier), "code_verifier");

            using (var resp = await _http.PostAsync(_tokenEndpoint, form, ct).ConfigureAwait(false))
            {
                var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!resp.IsSuccessStatusCode)
                    throw new HttpRequestException($"Token exchange failed: {(int)resp.StatusCode} {resp.ReasonPhrase}\n{json}");

                var token = JsonConvert.DeserializeObject<TokenResponse>(json) ?? new TokenResponse();
                token.IssuedUtc = DateTimeOffset.UtcNow;
                await _store.SaveAsync(token).ConfigureAwait(false);
                return token;
            }
        }

        /// <summary>
        /// Returns a valid access token, refreshing if needed.
        /// </summary>
        public async Task<string> GetAccessTokenAsync(CancellationToken ct = default)
        {
            var token = await _store.LoadAsync().ConfigureAwait(false);
            if (token == null) throw new InvalidOperationException("No tokens stored. Call ExchangeCodeAsync first.");

            if (IsExpiredOrNear(token))
            {
                if (string.IsNullOrWhiteSpace(token.RefreshToken))
                    throw new InvalidOperationException("Access token expired and no refresh token available.");

                token = await RefreshAsync(token.RefreshToken, ct).ConfigureAwait(false);
            }

            return token.AccessToken;
        }

        /// <summary>
        /// Refreshes the access token using the stored (or provided) refresh token.
        /// </summary>
        public async Task<TokenResponse> RefreshAsync(string refreshToken = null, CancellationToken ct = default)
        {
            var existing = await _store.LoadAsync().ConfigureAwait(false);
            var rt = refreshToken ?? existing?.RefreshToken;
            if (string.IsNullOrWhiteSpace(rt)) throw new InvalidOperationException("Missing refresh token.");

            var form = new MultipartFormDataContent
            {
                { new StringContent("refresh_token"), "grant_type" },
                { new StringContent(rt), "refresh_token" },
                { new StringContent(_clientId), "client_id" }
            };
            if (!string.IsNullOrEmpty(_clientSecret))
                form.Add(new StringContent(_clientSecret), "client_secret");

            using (var resp = await _http.PostAsync(_tokenEndpoint, form, ct).ConfigureAwait(false))
            {
                var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!resp.IsSuccessStatusCode)
                    throw new HttpRequestException($"Token refresh failed: {(int)resp.StatusCode} {resp.ReasonPhrase}\n{json}");

                var token = JsonConvert.DeserializeObject<TokenResponse>(json) ?? new TokenResponse();
                token.IssuedUtc = DateTimeOffset.UtcNow;

                // If refresh_token wasn't included in response, keep the old one (per RFC).
                if (string.IsNullOrWhiteSpace(token.RefreshToken))
                    token.RefreshToken = existing?.RefreshToken;

                await _store.SaveAsync(token).ConfigureAwait(false);
                return token;
            }
        }

        /// <summary>
        /// Helper: sets Bearer header with a valid token (auto-refreshes if needed).
        /// </summary>
        public async Task AttachBearerAsync(HttpClient client, CancellationToken ct = default)
        {
            var access = await GetAccessTokenAsync(ct).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access);
        }

        public void Dispose() => _http?.Dispose();

        private bool IsExpiredOrNear(TokenResponse t)
        {
            if (string.IsNullOrWhiteSpace(t.AccessToken)) return true;

            // If no explicit expires_in, assume 1 hour from issue time
            var lifetime = t.ExpiresIn > 0 ? TimeSpan.FromSeconds(t.ExpiresIn) : TimeSpan.FromHours(1);
            var expiry = t.IssuedUtc + lifetime;

            return DateTimeOffset.UtcNow + _expirySkew >= expiry;
        }
    }

    // ===== Token models & store =====

    public sealed class TokenResponse
    {
        [JsonProperty("access_token")] public string AccessToken { get; set; }
        [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
        [JsonProperty("token_type")] public string TokenType { get; set; }
        [JsonProperty("expires_in")] public int ExpiresIn { get; set; }
        [JsonProperty("scope")] public string Scope { get; set; }

        // Local bookkeeping
        [JsonIgnore] public DateTimeOffset IssuedUtc { get; set; }
    }

    public interface ITokenStore
    {
        Task SaveAsync(TokenResponse token);
        Task<TokenResponse> LoadAsync();
    }

    /// <summary>Volatile in-memory store (process lifetime only).</summary>
    public sealed class InMemoryTokenStore : ITokenStore
    {
        private TokenResponse _token;
        public Task SaveAsync(TokenResponse token) { _token = token; return Task.CompletedTask; }
        public Task<TokenResponse> LoadAsync() => Task.FromResult(_token);
    }

    /// <summary>Simple JSON file store (plaintext on disk!).</summary>
    public sealed class FileTokenStore : ITokenStore
    {
        private readonly string _path;
        public FileTokenStore(string path) { _path = path; }
        public async Task SaveAsync(TokenResponse token)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            var json = JsonConvert.SerializeObject(token, Formatting.Indented);
            //await File.WriteAllTextAsync(_path, json).ConfigureAwait(false);
            File.WriteAllText(_path, json);
        }
        public async Task<TokenResponse> LoadAsync()
        {
            if (!File.Exists(_path)) return null;
            //var json = await File.ReadAllTextAsync(_path).ConfigureAwait(false);
            var json = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<TokenResponse>(json);
        }
    }

    // ===== Optional: tiny PKCE helper (create challenge & verifier) =====
    public static class Pkce
    {
        public static void Create(out string verifier, out string challenge)
        {
            // Very small PKCE helper; replace with a stronger/random implementation for production.
            // RFC 7636: code_verifier (43..128 chars) of [A-Z / a-z / 0-9 / "-" / "." / "_" / "~"]
            var guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                        .TrimEnd('=').Replace('+', '-').Replace('/', '_');
            verifier = guid + guid; // lengthen
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var hash = sha.ComputeHash(Encoding.ASCII.GetBytes(verifier));
                challenge = Convert.ToBase64String(hash).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            }
        }
    }
}

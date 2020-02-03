using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http.Auth
{
    [PublicAPI]
    public class JwtHttpAuthenticator : IHttpAuthenticator
    {
        private readonly HttpClient _httpClient;

        private AuthToken _authToken;

        public JwtHttpAuthenticator(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AuthenticateAsync()
        {
            _authToken = await _httpClient.RequestJwtTokenAsync(TokenRequestUri, new TokenRequest{UserName = UserName, Password = Password})
                .ConfigureAwait(false);
        }

        public void PrepareHttpRequest(HttpRequestMessage httpRequest)
        {
            if (string.IsNullOrEmpty(_authToken?.Token))
            {
                return;
            }

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authToken?.Token);
        }

        public bool CanAuthenticate()
        {
            return !string.IsNullOrWhiteSpace(UserName);
        }

        public Uri TokenRequestUri { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
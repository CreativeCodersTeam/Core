using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace CreativeCoders.Net.Http.Auth
{
    [PublicAPI]
    public static class HttpClientAuthExtensions
    {
        public static async Task<AuthToken> RequestJwtTokenAsync(this HttpClient httpClient, Uri requestUri, TokenRequest tokenRequest, string tokenPropertyName)
        {
            var response =
                await HttpClientExtensions.ExecuteHttpActionWithEnsureSuccessStatus(httpClient.PostJsonAsync(requestUri, tokenRequest))
                    .ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            var jsonObject = JObject.Parse(responseContent);
            var tokenProperty = jsonObject
                .Properties()
                .FirstOrDefault(p =>
                    string.IsNullOrEmpty(tokenPropertyName) || p.Name.Equals(tokenPropertyName,
                        StringComparison.InvariantCultureIgnoreCase));

            if (tokenProperty == null)
            {
                throw new InvalidDataException(
                    $"RequestJwtToken response has not the specified token property '{tokenPropertyName}'");
            }

            var authToken = tokenProperty.Value.ToString();
            return new AuthToken(authToken);
        }

        public static Task<AuthToken> RequestJwtTokenAsync(this HttpClient httpClient, Uri requestUri,
            TokenRequest tokenRequest)
        {
            return httpClient.RequestJwtTokenAsync(requestUri, tokenRequest, string.Empty);
        }
    }
}
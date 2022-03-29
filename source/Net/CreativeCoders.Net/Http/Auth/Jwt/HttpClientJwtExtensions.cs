using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CreativeCoders.Net.Http.Auth.Jwt;

public static class HttpClientJwtExtensions
{
    public static async Task<JwtAuthToken> RequestJwtTokenAsync(this HttpClient httpClient, Uri requestUri,
        JwtTokenRequest tokenRequest, string tokenPropertyName)
    {
        using var response =
            await httpClient.PostAsJsonAsync(requestUri, tokenRequest).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

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
            throw new JwtTokenNotFoundException(tokenPropertyName, responseContent);
        }

        var authToken = tokenProperty.Value.ToString();
        return new JwtAuthToken(authToken);
    }

    public static Task<JwtAuthToken> RequestJwtTokenAsync(this HttpClient httpClient, Uri requestUri,
        JwtTokenRequest tokenRequest)
    {
        return httpClient.RequestJwtTokenAsync(requestUri, tokenRequest, string.Empty);
    }
}
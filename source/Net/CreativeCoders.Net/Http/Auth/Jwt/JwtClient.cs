using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Http.Auth.Jwt;

public class JwtClient : IJwtClient
{
    private readonly HttpClient _httpClient;

    public JwtClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> RequestTokenAsync(Uri requestUri, JwtTokenRequest tokenRequest)
    {
        var tokenInfo = await RequestTokenInfoAsync(requestUri, tokenRequest).ConfigureAwait(false);

        return tokenInfo.Token;
    }

    public async Task<JwtTokenInfo> RequestTokenInfoAsync(Uri requestUri, JwtTokenRequest tokenRequest)
    {
        var authToken = await _httpClient.RequestJwtTokenAsync(requestUri, tokenRequest).ConfigureAwait(false);

        return new JwtTokenInfo(authToken.Token);
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http.Auth.Jwt;

[PublicAPI]
public class JwtHttpClientAuthenticator : IJwtHttpClientAuthenticator
{
    private readonly IJwtClient _jwtClient;

    private JwtTokenInfo _authToken;

    public JwtHttpClientAuthenticator(IJwtClient jwtClient)
    {
        Ensure.IsNotNull(jwtClient, nameof(jwtClient));

        _jwtClient = jwtClient;
    }

    public async Task AuthenticateAsync(Uri requestUri)
    {
        if (!CanAuthenticate(requestUri))
        {
            return;
        }

        var loginCredentials = Credentials.GetCredential(requestUri, "JWT");

        _authToken = await _jwtClient.RequestTokenInfoAsync(
                TokenRequestUri,
                new JwtTokenRequest
                {
                    UserName = loginCredentials?.UserName,
                    Password = loginCredentials?.Password,
                    Domain = loginCredentials?.Domain
                })
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

    public bool CanAuthenticate(Uri requestUri)
    {
        var credentials = Credentials?.GetCredential(requestUri, "JWT");

        return credentials != null && !string.IsNullOrEmpty(credentials.UserName) &&
               !string.IsNullOrEmpty(credentials.Password) && TokenRequestUri != null;
    }

    public Uri TokenRequestUri { get; set; }

    public ICredentials Credentials { get; set; }
}
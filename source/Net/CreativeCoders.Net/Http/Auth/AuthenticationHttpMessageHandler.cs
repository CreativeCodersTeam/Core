using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace CreativeCoders.Net.Http.Auth;

public class AuthenticationHttpMessageHandler : DelegatingHandler
{
    private IHttpClientAuthenticationProvider _authenticationProvider;

    public AuthenticationHttpMessageHandler(HttpMessageHandler  httpMessageHandler) : base(httpMessageHandler)
    {
    }

    public void SetAuthenticationProvider(IHttpClientAuthenticationProvider authenticationProvider)
    {
        _authenticationProvider = authenticationProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await Policy
            .HandleResult<HttpResponseMessage>(x =>
                x.StatusCode == HttpStatusCode.Unauthorized &&
                _authenticationProvider?.ClientAuthenticator?.CanAuthenticate(request.RequestUri) == true)
            .RetryAsync(1,
                async (_, _) => await RetryAsync(request))
            .ExecuteAsync(async () => await base.SendAsync(request, cancellationToken));
    }

    private async Task RetryAsync(HttpRequestMessage request)
    {
        var authenticator = _authenticationProvider.ClientAuthenticator;

        if (authenticator == null)
        {
            throw new ArgumentException("Authenticator not set.");
        }

        await authenticator.AuthenticateAsync(request.RequestUri);

        authenticator.PrepareHttpRequest(request);
    }
}
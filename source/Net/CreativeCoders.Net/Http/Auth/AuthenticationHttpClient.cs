using System.Net.Http;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http.Auth;

[PublicAPI]
public class AuthenticationHttpClient : HttpClient, IHttpClientAuthenticationProvider
{
    private IHttpClientAuthenticator _clientAuthenticator = new NullHttpClientAuthenticator();

    public AuthenticationHttpClient(IHttpMessageHandlerFactory httpMessageHandlerFactory)
        : this(new AuthenticationHttpMessageHandler(httpMessageHandlerFactory.CreateHandler())) { }

    internal AuthenticationHttpClient(IHttpMessageHandlerFactory httpMessageHandlerFactory, string name)
        : this(new AuthenticationHttpMessageHandler(httpMessageHandlerFactory.CreateHandler(name))) { }

    private AuthenticationHttpClient(AuthenticationHttpMessageHandler httpMessageHandler) : base(
        httpMessageHandler, false)
    {
        httpMessageHandler.SetAuthenticationProvider(this);
    }

    public IHttpClientAuthenticator ClientAuthenticator
    {
        get => _clientAuthenticator;
        set => _clientAuthenticator = value ?? NullHttpClientAuthenticator.Default;
    }
}

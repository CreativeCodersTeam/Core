using JetBrains.Annotations;

namespace CreativeCoders.Net.Http.Auth;

[PublicAPI]
public interface IAuthenticationHttpClientFactory
{
    AuthenticationHttpClient CreateClient(string name);

    AuthenticationHttpClient CreateClient();
}
namespace CreativeCoders.Net.Http.Auth
{
    public interface IAuthenticationHttpClientFactory
    {
        AuthenticationHttpClient CreateClient(string name);

        AuthenticationHttpClient CreateClient();
    }
}
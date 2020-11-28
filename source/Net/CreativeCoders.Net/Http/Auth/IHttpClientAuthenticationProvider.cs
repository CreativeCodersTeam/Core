namespace CreativeCoders.Net.Http.Auth
{
    public interface IHttpClientAuthenticationProvider
    {
        IHttpClientAuthenticator ClientAuthenticator { get; }
    }
}
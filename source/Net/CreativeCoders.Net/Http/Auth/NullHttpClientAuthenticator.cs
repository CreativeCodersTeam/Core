using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Http.Auth;

public class NullHttpClientAuthenticator : IHttpClientAuthenticator
{
    public static readonly IHttpClientAuthenticator Default = new NullHttpClientAuthenticator();

    public Task AuthenticateAsync(Uri requestUri)
    {
        throw new NotSupportedException();
    }

    public void PrepareHttpRequest(HttpRequestMessage httpRequest) { }

    public bool CanAuthenticate(Uri requestUri) => false;
}

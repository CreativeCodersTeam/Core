using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Http.Auth
{
    public interface IHttpClientAuthenticator
    {
        Task AuthenticateAsync(Uri requestUri);

        void PrepareHttpRequest(HttpRequestMessage httpRequest);

        bool CanAuthenticate(Uri requestUri);
    }
}
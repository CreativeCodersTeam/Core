using System.Net.Http;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Http.Auth
{
    public interface IHttpAuthenticator
    {
        Task AuthenticateAsync();

        void PrepareHttpRequest(HttpRequestMessage httpRequest);

        bool CanAuthenticate();
    }
}
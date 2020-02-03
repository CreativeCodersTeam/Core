using System.Net.Http;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Http.Auth
{
    public class NullHttpAuthenticator : IHttpAuthenticator
    {
        public Task AuthenticateAsync()
        {
            return Task.CompletedTask;
        }

        public void PrepareHttpRequest(HttpRequestMessage httpRequest)
        {
            
        }

        public bool CanAuthenticate() => false;
    }
}
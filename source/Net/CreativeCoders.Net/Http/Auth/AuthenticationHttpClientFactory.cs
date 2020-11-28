using System.Net.Http;
using CreativeCoders.Core;
using Microsoft.Extensions.Options;

namespace CreativeCoders.Net.Http.Auth
{
    public class AuthenticationHttpClientFactory : IAuthenticationHttpClientFactory
    {
        private readonly IHttpMessageHandlerFactory _httpMessageHandlerFactory;

        public AuthenticationHttpClientFactory(IHttpMessageHandlerFactory httpMessageHandlerFactory)
        {
            Ensure.IsNotNull(httpMessageHandlerFactory, nameof(httpMessageHandlerFactory));

            _httpMessageHandlerFactory = httpMessageHandlerFactory;
        }

        public AuthenticationHttpClient CreateClient(string name)
        {
            return new AuthenticationHttpClient(_httpMessageHandlerFactory, name);
        }

        public AuthenticationHttpClient CreateClient()
        {
            return CreateClient(Options.DefaultName);
        }
    }
}
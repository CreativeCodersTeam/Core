using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CreativeCoders.Net.Http.Auth;
using CreativeCoders.Net.Http.Auth.Jwt;
using Microsoft.Extensions.DependencyInjection;

namespace NetSampleApp
{
    public class HttpClientAuthTest
    {
        public async Task Run()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            
            serviceCollection.AddTransient<AuthenticationHttpClient>();
            serviceCollection.AddTransient<IJwtClient, JwtClient>();

            var sp = serviceCollection.BuildServiceProvider();

            await TestAuthHttpClient(sp);
        }

        private async Task TestAuthHttpClient(ServiceProvider sp)
        {
            var clients = new List<HttpClient>();

            foreach (var _ in Enumerable.Range(0, 1))
            {
                var authClient = sp.GetRequiredService<AuthenticationHttpClient>();

                var jwtClientAuthenticator = sp.GetRequiredService<IJwtHttpClientAuthenticator>();
                jwtClientAuthenticator.TokenRequestUri = new Uri("http://localhost:5000/auth/tokenauth/requesttoken");
                jwtClientAuthenticator.Credentials = new NetworkCredential("user", "pass", "domain");

                authClient.ClientAuthenticator = jwtClientAuthenticator;

                var firstResult = await authClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>(new Uri("http://localhost:5000/WeatherForecast"));

                clients.Add(authClient);
            }
        }
    }
}
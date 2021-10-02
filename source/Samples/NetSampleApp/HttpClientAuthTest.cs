using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.Net.Http.Auth;
using CreativeCoders.Net.Http.Auth.Jwt;
using Microsoft.Extensions.DependencyInjection;

namespace NetSampleApp
{
    public class HttpClientAuthTest
    {
        private const string TokenRequestUrl = "http://localhost:5000/auth/tokenauth/requesttoken";
        
        public async Task Run()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            
            serviceCollection.AddTransient<AuthenticationHttpClient>();
            serviceCollection.AddTransient<IJwtClient, JwtClient>();

            var sp = serviceCollection.BuildServiceProvider();

            await TestAuthHttpClient(sp);
        }

        private static async Task TestAuthHttpClient(IServiceProvider sp)
        {
            var authClient = sp.GetRequiredService<AuthenticationHttpClient>();

            var jwtClientAuthenticator = sp.GetRequiredService<IJwtHttpClientAuthenticator>();
            jwtClientAuthenticator.TokenRequestUri = new Uri(TokenRequestUrl);
            jwtClientAuthenticator.Credentials = new NetworkCredential("user", "pass", "domain");

            authClient.ClientAuthenticator = jwtClientAuthenticator;

            var result = await authClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>(new Uri("http://localhost:5000/WeatherForecast"));

            result.ForEach(x => Console.WriteLine($"Summary: {x.Summary}"));
        }
    }
}

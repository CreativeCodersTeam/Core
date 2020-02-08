using System.Net.Http;
using CreativeCoders.Net.Http;
using CreativeCoders.Net.WebApi.Building;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi
{
    [PublicAPI]
    public class WebApiClientFactory : IWebApiClientFactory
    {
        public IWebApiClientBuilder<T> CreateBuilder<T>()
            where T : class
        {
            var apiBuilder = new ApiBuilder(new HttpClientEx(new HttpClient()));

            return new WebApiClientBuilder<T>(apiBuilder);
        }
    }
}
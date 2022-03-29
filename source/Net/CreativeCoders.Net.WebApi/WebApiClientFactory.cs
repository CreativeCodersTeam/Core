using System.Net.Http;
using CreativeCoders.Core;
using CreativeCoders.Net.WebApi.Building;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi;

[PublicAPI]
public class WebApiClientFactory : IWebApiClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;

    public WebApiClientFactory(IHttpClientFactory httpClientFactory)
    {
        Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

        _httpClientFactory = httpClientFactory;
    }

    public IWebApiClientBuilder<T> CreateBuilder<T>()
        where T : class
    {
        var apiBuilder = new ApiBuilder(_httpClientFactory.CreateClient());

        return new WebApiClientBuilder<T>(apiBuilder);
    }
}

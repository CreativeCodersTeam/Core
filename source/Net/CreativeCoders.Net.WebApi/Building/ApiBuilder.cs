using System.Net.Http;
using CreativeCoders.Core;
using CreativeCoders.DynamicCode.Proxying;
using CreativeCoders.Net.WebApi.Execution;
using CreativeCoders.Net.WebApi.Serialization;

namespace CreativeCoders.Net.WebApi.Building;

internal class ApiBuilder : IApiBuilder
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly IProxyBuilderFactory _proxyBuilderFactory;

    public ApiBuilder(IHttpClientFactory httpClientFactory, IProxyBuilderFactory proxyBuilderFactory)
    {
        _httpClientFactory = Ensure.NotNull(httpClientFactory, nameof(httpClientFactory));
        _proxyBuilderFactory = Ensure.NotNull(proxyBuilderFactory, nameof(proxyBuilderFactory));
    }

    public T BuildApi<T>(string baseUri, IDataFormatter defaultDataFormatter)
        where T : class
    {
        var apiData = new ApiData
        {
            BaseUri = baseUri,
            HttpClient = _httpClientFactory.CreateClient(),
            DefaultDataFormatter = defaultDataFormatter
        };

        var apiStructure = new ApiAnalyzer<T>().Analyze();

        var interceptor = new ApiInvocator<T>(apiData, apiStructure);
        var api = _proxyBuilderFactory.Create<T>().Build(interceptor);

        return api;
    }
}

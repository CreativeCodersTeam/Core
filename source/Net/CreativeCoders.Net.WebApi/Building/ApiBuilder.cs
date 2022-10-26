using System.Net.Http;
using CreativeCoders.Core;
using CreativeCoders.DynamicCode.Proxying;
using CreativeCoders.Net.WebApi.Execution;
using CreativeCoders.Net.WebApi.Serialization;

namespace CreativeCoders.Net.WebApi.Building;

internal class ApiBuilder : IApiBuilder
{
    private readonly HttpClient _httpClient;

    private readonly IProxyBuilderFactory _proxyBuilderFactory;

    public ApiBuilder(HttpClient httpClient, IProxyBuilderFactory proxyBuilderFactory)
    {
        _httpClient = Ensure.NotNull(httpClient, nameof(httpClient));
        _proxyBuilderFactory = Ensure.NotNull(proxyBuilderFactory, nameof(proxyBuilderFactory));
    }

    public T BuildApi<T>(string baseUri, IDataFormatter defaultDataFormatter)
        where T : class
    {
        var apiData = new ApiData
        {
            BaseUri = baseUri,
            HttpClient = _httpClient,
            DefaultDataFormatter = defaultDataFormatter
        };

        var apiStructure = new ApiAnalyzer<T>().Analyze();

        var interceptor = new ApiInvocator<T>(apiData, apiStructure);
        var api = _proxyBuilderFactory.Create<T>().Build(interceptor);

        return api;
    }
}

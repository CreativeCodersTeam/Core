using System.Net.Http;
using CreativeCoders.DynamicCode.Proxying;
using CreativeCoders.Net.WebApi.Execution;
using CreativeCoders.Net.WebApi.Serialization;

namespace CreativeCoders.Net.WebApi.Building
{
    public class ApiBuilder : IApiBuilder
    {
        private readonly HttpClient _httpClient;

        public ApiBuilder(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
            var api = new ProxyBuilder<T>().Build(interceptor);

            return api;
        }
    }
}
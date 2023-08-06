using System.Net.Http;
using CreativeCoders.Core;

namespace CreativeCoders.Net.JsonRpc;

public class JsonRpcClientFactory : IJsonRpcClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;

    public JsonRpcClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = Ensure.NotNull(httpClientFactory, nameof(httpClientFactory));
    }

    public IJsonRpcClient Create(string name)
    {
        return new JsonRpcClient(_httpClientFactory.CreateClient(name));
    }

    public IJsonRpcClient Create()
    {
        return new JsonRpcClient(_httpClientFactory.CreateClient());
    }
}

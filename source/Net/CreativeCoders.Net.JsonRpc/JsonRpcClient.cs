using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CreativeCoders.Core;

namespace CreativeCoders.Net.JsonRpc;

public class JsonRpcClient : IJsonRpcClient
{
    private readonly HttpClient _httpClient;

    public JsonRpcClient(HttpClient httpClient)
    {
        _httpClient = Ensure.NotNull(httpClient, nameof(httpClient));
    }

    public async Task<JsonRpcResponse<T>> ExecuteAsync<T>(Uri url, string methodName, params object?[] arguments)
    {
        var jsonRpcRequest = new JsonRpcRequest(methodName, arguments);

        var httpResponse = await _httpClient.PostAsJsonAsync(url, jsonRpcRequest).ConfigureAwait(false);

        httpResponse.EnsureSuccessStatusCode();

        var jsonRpcResponse = await httpResponse.Content
            .ReadFromJsonAsync<JsonRpcResponse<T>>()
            .ConfigureAwait(false);

        if (jsonRpcResponse == null)
        {
            throw new InvalidOperationException();
        }

        if (jsonRpcResponse.Id != jsonRpcRequest.Id)
        {
            throw new InvalidOperationException("Json RPC id mismatch");
        }

        return jsonRpcResponse;
    }
}

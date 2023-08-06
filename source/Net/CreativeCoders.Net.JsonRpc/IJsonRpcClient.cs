using System;
using System.Threading.Tasks;

namespace CreativeCoders.Net.JsonRpc;

public interface IJsonRpcClient
{
    Task<JsonRpcResponse<T>> ExecuteAsync<T>(Uri url, string methodName, params object?[] arguments);
}

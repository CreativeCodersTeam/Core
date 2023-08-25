using CreativeCoders.Core;

namespace CreativeCoders.Net.JsonRpc;

public static class JsonRpcResponseExtensions
{
    public static void EnsureSuccess<T>(this JsonRpcResponse<T> response, string? methodName = null)
    {
        Ensure.NotNull(response);

        if (response.Error != null)
        {
            throw new JsonRpcCallException(response.Error.Code, response.Error.Message, methodName);
        }
    }
}

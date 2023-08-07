using System;

namespace CreativeCoders.Net.JsonRpc;

public class JsonRpcCallException : Exception
{
    public JsonRpcCallException(int errorCode, string? errorMessage, string? rpcMethodName)
        : base($"Json RPC method '{rpcMethodName}' call failed ({errorCode}): {errorMessage}")
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        RpcMethodName = rpcMethodName;
    }

    public int ErrorCode { get; }

    public string? ErrorMessage { get; }

    public string? RpcMethodName { get; }
}

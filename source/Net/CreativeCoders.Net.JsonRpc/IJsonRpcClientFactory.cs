using JetBrains.Annotations;

namespace CreativeCoders.Net.JsonRpc;

[PublicAPI]
public interface IJsonRpcClientFactory
{
    IJsonRpcClient Create(string name);

    IJsonRpcClient Create();
}

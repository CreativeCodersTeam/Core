namespace CreativeCoders.Net.JsonRpc;

public interface IJsonRpcClientFactory
{
    IJsonRpcClient Create(string name);
    
    IJsonRpcClient Create();
}
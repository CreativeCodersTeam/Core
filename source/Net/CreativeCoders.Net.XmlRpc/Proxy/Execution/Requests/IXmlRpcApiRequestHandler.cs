using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy.Execution.Requests;

public interface IXmlRpcApiRequestHandler
{
    ApiMethodReturnType ReturnType { get; }

    object HandleRequest(RequestData requestData);
}
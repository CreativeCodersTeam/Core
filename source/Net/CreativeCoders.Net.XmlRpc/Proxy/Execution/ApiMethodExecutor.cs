using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using CreativeCoders.Net.XmlRpc.Client;
using CreativeCoders.Net.XmlRpc.Exceptions;
using CreativeCoders.Net.XmlRpc.Proxy.Execution.Requests;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy.Execution;

public class ApiMethodExecutor : IApiMethodExecutor
{
    private readonly IXmlRpcClient _xmlRpcClient;

    private readonly IList<IXmlRpcApiRequestHandler> _requestHandlers;

    public ApiMethodExecutor(IXmlRpcClient xmlRpcClient)
    {
        _xmlRpcClient = xmlRpcClient;

        _requestHandlers = new List<IXmlRpcApiRequestHandler>
        {
            new VoidRequestHandler(),
            new XmlRpcValueRequestHandler(),
            new ValueRequestHandler(),
            new GenericValueRequestHandler(),
            new ObjectValueRequestHandler(),
            new EnumerableRequestHandler(),
            new DictionaryRequestHandler()
        };
    }

    public object Execute(ApiMethodInfo apiMethodInfo, IInvocation invocation)
    {
        var requestHandler = _requestHandlers.FirstOrDefault(x => x.ReturnType == apiMethodInfo.ReturnType);

        if (requestHandler == null)
        {
            throw new ReturnTypeNotSupportedException(apiMethodInfo.ReturnType);
        }

        return requestHandler.HandleRequest(
            new RequestData
            {
                ValueType = apiMethodInfo.ValueType,
                Arguments = invocation.Arguments,
                Client = _xmlRpcClient,
                InvocationMethod = invocation.Method,
                MethodName = apiMethodInfo.MethodName,
                ExceptionHandler = apiMethodInfo.ExceptionHandler,
                DefaultResult = apiMethodInfo.DefaultResult
            });
    }
}

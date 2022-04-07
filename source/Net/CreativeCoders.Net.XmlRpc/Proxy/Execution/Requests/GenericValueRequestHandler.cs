using System.Linq;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy.Execution.Requests;

public class GenericValueRequestHandler : XmlRpcRequestHandlerBase
{
    private readonly ValueRequestHandler _valueRequestHandler;

    private readonly XmlRpcValueRequestHandler _xmlRpcValueRequestHandler;

    private readonly ObjectValueRequestHandler _objectValueRequestHandler;

    public GenericValueRequestHandler() : base(ApiMethodReturnType.GenericValue)
    {
        _valueRequestHandler = new ValueRequestHandler();
        _xmlRpcValueRequestHandler = new XmlRpcValueRequestHandler();
        _objectValueRequestHandler = new ObjectValueRequestHandler();
    }

    public override object HandleRequest(RequestData requestData)
    {
        var dataType = requestData.InvocationMethod.ReturnType.GetGenericArguments().First();

        requestData.ValueType = dataType;

        if (typeof(XmlRpcValue).IsAssignableFrom(dataType))
        {
            return _xmlRpcValueRequestHandler.HandleRequest(requestData);
        }

        return dataType.IsValueType
            ? _valueRequestHandler.HandleRequest(requestData)
            : _objectValueRequestHandler.HandleRequest(requestData);
    }
}

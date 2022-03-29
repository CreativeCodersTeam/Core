using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.Net.XmlRpc.Model;

namespace CreativeCoders.Net.XmlRpc;

public class XmlRpcRequest
{
    private readonly IList<XmlRpcMethodCall> _methods;

    public XmlRpcRequest(IEnumerable<XmlRpcMethodCall> methods, bool isMultiCall)
    {
        var methodsArray = methods.ToArray();
        Ensure.IsNotNull(methodsArray, nameof(methods));

        _methods = new List<XmlRpcMethodCall>(methodsArray);
        IsMultiCall = isMultiCall;
    }

    public IEnumerable<XmlRpcMethodCall> Methods => _methods;

    public bool IsMultiCall { get; }
}

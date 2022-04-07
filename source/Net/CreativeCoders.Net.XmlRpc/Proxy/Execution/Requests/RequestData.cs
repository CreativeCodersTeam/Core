using System;
using System.Reflection;
using CreativeCoders.Net.XmlRpc.Client;
using CreativeCoders.Net.XmlRpc.Definition;

namespace CreativeCoders.Net.XmlRpc.Proxy.Execution.Requests;

public class RequestData
{
    public string MethodName { get; set; }

    public Type ValueType { get; set; }

    public object[] Arguments { get; set; }

    public IXmlRpcClient Client { get; set; }

    public MethodInfo InvocationMethod { get; set; }

    public IMethodExceptionHandler ExceptionHandler { get; set; }

    public object DefaultResult { get; set; }
}

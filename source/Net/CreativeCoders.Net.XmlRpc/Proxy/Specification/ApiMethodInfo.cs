using System;
using System.Reflection;
using CreativeCoders.Net.XmlRpc.Definition;

namespace CreativeCoders.Net.XmlRpc.Proxy.Specification;

public class ApiMethodInfo
{
    public MethodInfo Method { get; init; }

    public ApiMethodReturnType ReturnType { get; set; }

    public string MethodName { get; init; }

    public Type ValueType { get; set; }

    public IMethodExceptionHandler ExceptionHandler { get; init; }

    public object DefaultResult { get; init; }
}

using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Definition;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method)]
public class XmlRpcMethodAttribute : Attribute
{
    public XmlRpcMethodAttribute() : this(string.Empty) { }

    public XmlRpcMethodAttribute(string methodName)
    {
        MethodName = methodName;
    }

    public string MethodName { get; }

    public Type ExceptionHandler { get; set; }

    public object DefaultResult { get; set; }
}

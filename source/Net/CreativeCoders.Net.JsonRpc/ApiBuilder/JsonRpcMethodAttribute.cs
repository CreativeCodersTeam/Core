using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.JsonRpc.ApiBuilder;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method)]
public class JsonRpcMethodAttribute : Attribute
{
    public JsonRpcMethodAttribute(string methodName)
    {
        MethodName = methodName;
    }

    public string MethodName { get; }
}

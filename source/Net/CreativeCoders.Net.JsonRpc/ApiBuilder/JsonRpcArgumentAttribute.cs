using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.JsonRpc.ApiBuilder;

[PublicAPI]
[AttributeUsage(AttributeTargets.Parameter)]
public class JsonRpcArgumentAttribute : Attribute
{
    public JsonRpcArgumentAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}

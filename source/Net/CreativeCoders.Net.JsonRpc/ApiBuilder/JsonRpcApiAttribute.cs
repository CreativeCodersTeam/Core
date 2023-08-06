using System;

namespace CreativeCoders.Net.JsonRpc.ApiBuilder;

[AttributeUsage(AttributeTargets.Interface)]
public class JsonRpcApiAttribute : Attribute
{
    public bool IncludeParameterNames { get; set; }
}

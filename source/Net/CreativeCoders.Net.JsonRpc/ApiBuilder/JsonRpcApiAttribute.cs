using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.JsonRpc.ApiBuilder;

[PublicAPI]
[AttributeUsage(AttributeTargets.Interface)]
public class JsonRpcApiAttribute : Attribute
{
    public bool IncludeParameterNames { get; set; }
}

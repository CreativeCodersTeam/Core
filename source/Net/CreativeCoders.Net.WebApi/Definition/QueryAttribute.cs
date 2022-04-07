using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Definition;

[PublicAPI]
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class QueryAttribute : Attribute
{
    public QueryAttribute() { }

    public QueryAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public bool UrlEncode { get; set; }
}

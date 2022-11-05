using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Definition;

[PublicAPI]
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class PathAttribute : Attribute
{
    public string Name { get; set; }

    public bool UrlEncode { get; set; }
}

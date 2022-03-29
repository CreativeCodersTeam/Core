using System;

namespace CreativeCoders.Net.WebApi.Definition;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class PathAttribute : Attribute
{
    public string Name { get; set; }

    public bool UrlEncode { get; set; }
}
using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Definition;

[PublicAPI]
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Property,
    AllowMultiple = true)]
public class HeaderAttribute : Attribute
{
    public HeaderAttribute(string name)
    {
        Name = name;
    }

    public HeaderAttribute(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; }

    public string Value { get; }

    public SerializationKind SerializationKind { get; set; }
}

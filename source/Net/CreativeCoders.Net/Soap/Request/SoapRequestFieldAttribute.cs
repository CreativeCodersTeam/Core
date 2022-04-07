using System;

namespace CreativeCoders.Net.Soap.Request;

[AttributeUsage(AttributeTargets.Property)]
public class SoapRequestFieldAttribute : Attribute
{
    public SoapRequestFieldAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

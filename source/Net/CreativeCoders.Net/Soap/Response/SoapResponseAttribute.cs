using System;

namespace CreativeCoders.Net.Soap.Response;

[AttributeUsage(AttributeTargets.Class)]
public class SoapResponseAttribute : Attribute
{
    public SoapResponseAttribute(string name, string nameSpace)
    {
        Name = name;
        NameSpace = nameSpace;
    }

    public string Name { get; }

    public string NameSpace { get; }
}
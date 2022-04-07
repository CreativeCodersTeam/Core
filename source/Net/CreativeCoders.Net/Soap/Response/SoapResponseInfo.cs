using System;

namespace CreativeCoders.Net.Soap.Response;

public class SoapResponseInfo
{
    public SoapResponseInfo()
    {
        PropertyMappings = Array.Empty<PropertyFieldMapping>();
    }

    public string Name { get; init; }

    public string NameSpace { get; init; }

    public PropertyFieldMapping[] PropertyMappings { get; set; }
}

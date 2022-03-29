using System;

namespace CreativeCoders.Net.Soap.Response;

public class SoapResponseInfo
{
    public SoapResponseInfo()
    {
        PropertyMappings = Array.Empty<PropertyFieldMapping>();
    }

    public string Name { get; set; }

    public string NameSpace { get; set; }

    public PropertyFieldMapping[] PropertyMappings { get; set; }
}
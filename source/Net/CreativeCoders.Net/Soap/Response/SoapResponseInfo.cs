using System;

namespace CreativeCoders.Net.Soap.Response;

public class SoapResponseInfo
{
    public string Name { get; init; }

    public string NameSpace { get; init; }

    public PropertyFieldMapping[] PropertyMappings { get; set; } = Array.Empty<PropertyFieldMapping>();
}

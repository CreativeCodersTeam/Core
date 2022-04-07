using System;

namespace CreativeCoders.Net.Soap.Request;

internal class SoapRequestInfo
{
    public SoapRequestInfo()
    {
        PropertyMappings = Array.Empty<PropertyFieldMapping>();
    }

    public Uri Url { get; init; }

    public string ActionName { get; init; }

    public object Action { get; init; }

    public string ServiceNameSpace { get; init; }

    public PropertyFieldMapping[] PropertyMappings { get; set; }
}

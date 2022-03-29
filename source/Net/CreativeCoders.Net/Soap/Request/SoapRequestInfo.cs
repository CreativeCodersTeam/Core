using System;
using System.Net;

namespace CreativeCoders.Net.Soap.Request;

internal class SoapRequestInfo
{
    public SoapRequestInfo()
    {
        PropertyMappings = Array.Empty<PropertyFieldMapping>();
    }

    public string Url { get; set; }

    public ICredentials Credentials { get; set; }

    public string ActionName { get; set; }

    public object Action { get; set; }

    public string ServiceNameSpace { get; set; }

    public PropertyFieldMapping[] PropertyMappings { get; set; }
}

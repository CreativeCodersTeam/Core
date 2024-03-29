﻿using System;

namespace CreativeCoders.Net.Soap.Request;

internal class SoapRequestInfo
{
    public Uri Url { get; init; }

    public string ActionName { get; init; }

    public object Action { get; init; }

    public string ServiceNameSpace { get; init; }

    public PropertyFieldMapping[] PropertyMappings { get; set; } = Array.Empty<PropertyFieldMapping>();
}

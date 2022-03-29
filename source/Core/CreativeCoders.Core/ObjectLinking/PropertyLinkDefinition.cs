using System;
using System.Reflection;

namespace CreativeCoders.Core.ObjectLinking;

public class PropertyLinkDefinition
{
    public object Source { get; set; }

    public object Target { get; set; }

    public Type TargetType { get; set; }

    public PropertyInfo SourceProperty { get; set; }

    public string TargetPropertyName { get; set; }

    public LinkDirection LinkDirection { get; set; }

    public Type Converter { get; set; }

    public object ConverterParameter { get; set; }

    public bool InitWithTargetValue { get; set; }
}

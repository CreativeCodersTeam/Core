using System;
using System.Reflection;

namespace CreativeCoders.Core.ObjectLinking;

public class PropertyLinkDefinition
{
    public object Source { get; init; }

    public object Target { get; init; }

    public Type TargetType { get; init; }

    public PropertyInfo SourceProperty { get; init; }

    public string TargetPropertyName { get; init; }

    public LinkDirection LinkDirection { get; init; }

    public Type Converter { get; init; }

    public object ConverterParameter { get; init; }

    public bool InitWithTargetValue { get; init; }
}

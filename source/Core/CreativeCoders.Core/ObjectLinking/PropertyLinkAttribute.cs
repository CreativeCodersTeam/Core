using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.ObjectLinking;

[PublicAPI]
public class PropertyLinkAttribute : Attribute
{
    public PropertyLinkAttribute(Type targetType, string targetPropertyName)
    {
        TargetType = targetType;
        TargetPropertyName = targetPropertyName;
    }

    public Type TargetType { get; }

    public string TargetPropertyName { get; }

    public LinkDirection Direction { get; set; }

    public Type Converter { get; set; }

    public object ConverterParameter { get; set; }

    public bool InitWithTargetValue { get; set; }
}

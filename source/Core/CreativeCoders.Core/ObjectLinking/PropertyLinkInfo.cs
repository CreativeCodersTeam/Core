using System.Reflection;

namespace CreativeCoders.Core.ObjectLinking;

public class PropertyLinkInfo
{
    public object Source { get; init; }

    public object Target { get; init; }

    public PropertyInfo SourceProperty { get; init; }

    public PropertyInfo TargetProperty { get; init; }

    public LinkDirection Direction { get; set; }

    public IPropertyValueConverter Converter { get; init; }

    public object ConverterParameter { get; init; }

    public bool InitWithTargetValue { get; init; }
}

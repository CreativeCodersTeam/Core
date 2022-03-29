using System.Reflection;

namespace CreativeCoders.Core.ObjectLinking;

public class PropertyLinkInfo
{
    public object Source { get; set; }

    public object Target { get; set; }

    public PropertyInfo SourceProperty { get; set; }

    public PropertyInfo TargetProperty { get; set; }

    public LinkDirection Direction { get; set; }

    public IPropertyValueConverter Converter { get; set; }

    public object ConverterParameter { get; set; }

    public bool InitWithTargetValue { get; set; }
}
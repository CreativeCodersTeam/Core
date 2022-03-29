using System.Reflection;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Specification.Properties;

[PublicAPI]
public abstract class PropertyDefinitionBase
{
    protected PropertyDefinitionBase(PropertyInfo propertyInfo)
    {
        PropertyInfo = propertyInfo;
    }

    public PropertyInfo PropertyInfo { get; }

    public string GetValue(object target)
    {
        return PropertyInfo.GetValue(target).ToStringSafe();
    }
}
using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Specification.Properties;

[PublicAPI]
public class PropertyPathDefinition : PropertyDefinitionBase
{
    public PropertyPathDefinition(PropertyInfo propertyInfo, bool urlEncode, string name) : base(propertyInfo)
    {
        UrlEncode = urlEncode;
        Name = string.IsNullOrWhiteSpace(name)
            ? propertyInfo.Name
            : name;
    }

    public bool UrlEncode { get; }

    public string Name { get; }
}
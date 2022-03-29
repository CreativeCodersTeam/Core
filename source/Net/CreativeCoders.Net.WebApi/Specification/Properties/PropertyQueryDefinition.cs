using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Specification.Properties;

[PublicAPI]
public class PropertyQueryDefinition : PropertyDefinitionBase
{
    public PropertyQueryDefinition(string name, bool urlEncode, PropertyInfo propertyInfo) : base(
        propertyInfo)
    {
        Name = name;
        UrlEncode = urlEncode;
    }

    public string Name { get; }

    public bool UrlEncode { get; }
}

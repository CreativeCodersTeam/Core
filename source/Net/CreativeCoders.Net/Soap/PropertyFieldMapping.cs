using System.Reflection;

namespace CreativeCoders.Net.Soap;

public class PropertyFieldMapping
{
    public string FieldName { get; init; }

    public PropertyInfo Property { get; init; }
}

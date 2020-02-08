using System.Reflection;

namespace CreativeCoders.Net.Soap
{
    public class PropertyFieldMapping
    {
        public string FieldName { get; set; }

        public PropertyInfo Property { get; set; }
    }
}
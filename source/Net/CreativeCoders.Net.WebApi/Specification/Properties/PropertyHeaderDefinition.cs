using System.Reflection;
using CreativeCoders.Net.WebApi.Definition;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Specification.Properties
{
    [PublicAPI]
    public class PropertyHeaderDefinition : PropertyDefinitionBase
    {
        public PropertyHeaderDefinition(string name, PropertyInfo propertyInfo, SerializationKind serializationKind) : base(propertyInfo)
        {
            Name = name;
            SerializationKind = serializationKind;
        }

        public string Name { get; }

        public SerializationKind SerializationKind { get; }
    }
}
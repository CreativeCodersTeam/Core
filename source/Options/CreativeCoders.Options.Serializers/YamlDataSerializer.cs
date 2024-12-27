using System.Reflection;
using CreativeCoders.Core;
using CreativeCoders.Options.Core;
using JetBrains.Annotations;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace CreativeCoders.Options.Serializers;

[UsedImplicitly]
public class YamlDataSerializer<T> : IOptionsStorageDataSerializer<T>
    where T : class
{
    private static readonly ISerializer __yamlSerializer = new SerializerBuilder().Build();

    private static void DeserializeInternal(string data, T options)
    {
        using var reader = new StringReader(data);
        var yamlStream = new YamlStream();
        yamlStream.Load(reader);

        var rootNode = (YamlMappingNode)yamlStream.Documents[0].RootNode;

        foreach (var entry in rootNode.Children)
        {
            var propertyName = ((YamlScalarNode)entry.Key).Value;

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                continue;
            }

            var propertyValue = ((YamlScalarNode)entry.Value).Value;

            var property = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)
                           ?? typeof(T).GetProperty(propertyName,
                               BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (property != null && property.CanWrite)
            {
                var convertedValue = Convert.ChangeType(propertyValue, property.PropertyType);
                property.SetValue(options, convertedValue);
            }
        }
    }

    public string Serialize(T options)
    {
        return __yamlSerializer.Serialize(options);
    }

    public void Deserialize(string data, T options)
    {
        Ensure.NotNull(data);

        if (string.IsNullOrWhiteSpace(data))
        {
            return;
        }

        DeserializeInternal(data, options);
    }
}

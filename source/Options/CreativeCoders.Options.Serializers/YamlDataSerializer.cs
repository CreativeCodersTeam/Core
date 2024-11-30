using CreativeCoders.Options.Core;
using JetBrains.Annotations;
using YamlDotNet.Serialization;

namespace CreativeCoders.Options.Serializers;

[UsedImplicitly]
public class YamlDataSerializer : IOptionsStorageDataSerializer
{
    private readonly IDeserializer _yamlDeserializer = new DeserializerBuilder().Build();

    private readonly ISerializer _yamlSerializer = new SerializerBuilder().Build();

    public string Serialize<T>(T options) where T : class
    {
        return _yamlSerializer.Serialize(options);
    }

    public void Deserialize<T>(string data, T options) where T : class
    {
        var deserializedOptions = _yamlDeserializer.Deserialize<T>(data);

        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            var value = property.GetValue(deserializedOptions);

            property.SetValue(options, value);
        }
    }
}

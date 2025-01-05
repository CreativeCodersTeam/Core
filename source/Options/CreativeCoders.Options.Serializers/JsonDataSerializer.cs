using System.Text.Json;
using CreativeCoders.Core;
using CreativeCoders.Core.Text;
using CreativeCoders.Options.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Options.Serializers;

[PublicAPI]
public class JsonDataSerializer<T> : JsonDataSerializerBase, IOptionsStorageDataSerializer<T>
    where T : class
{
    private readonly JsonSerializerOptions? _jsonSerializerOptions;

    public JsonDataSerializer() { }

    public JsonDataSerializer(JsonSerializerOptions jsonSerializerOptions)
    {
        Ensure.NotNull(jsonSerializerOptions);

        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public string Serialize(T options)
    {
        return JsonSerializer.Serialize(options, _jsonSerializerOptions ?? DefaultJsonSerializerOptions);
    }

    public void Deserialize(string data, T options)
    {
        Ensure.NotNull(data);

        if (string.IsNullOrWhiteSpace(data))
        {
            return;
        }

        data.PopulateJson(options, _jsonSerializerOptions ?? DefaultJsonSerializerOptions);
    }
}

using System.Text.Json;
using System.Text.Json.Serialization;
using CreativeCoders.Core;
using CreativeCoders.Core.Text;
using CreativeCoders.Options.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Options.Serializers;

[PublicAPI]
public class JsonDataSerializer<T> : IOptionsStorageDataSerializer<T>
    where T : class
{
    private static JsonSerializerOptions __jsonSerializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly JsonSerializerOptions? _jsonSerializerOptions;

    public JsonDataSerializer() { }

    public JsonDataSerializer(JsonSerializerOptions jsonSerializerOptions)
    {
        Ensure.NotNull(jsonSerializerOptions);

        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public string Serialize(T options)
    {
        return JsonSerializer.Serialize(options, _jsonSerializerOptions ?? __jsonSerializerOptions);
    }

    public void Deserialize(string data, T options)
    {
        Ensure.NotNull(data);

        if (string.IsNullOrWhiteSpace(data))
        {
            return;
        }

        data.PopulateJson(options, _jsonSerializerOptions ?? __jsonSerializerOptions);
    }
}

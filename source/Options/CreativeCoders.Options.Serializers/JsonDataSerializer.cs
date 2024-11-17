using System.Text.Json;
using System.Text.Json.Serialization;
using CreativeCoders.Core;
using CreativeCoders.Core.Text;
using CreativeCoders.Options.Core;

namespace CreativeCoders.Options.Serializers;

public class JsonDataSerializer : IOptionsStorageDataSerializer
{
    private static JsonSerializerOptions __jsonSerializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public JsonDataSerializer() { }

    public JsonDataSerializer(JsonSerializerOptions jsonSerializerOptions)
    {
        Ensure.NotNull(jsonSerializerOptions);

        __jsonSerializerOptions = jsonSerializerOptions;
    }

    public string Serialize<T>(T options) where T : class
    {
        return JsonSerializer.Serialize(options, __jsonSerializerOptions);
    }

    public void Deserialize<T>(string data, T options)
        where T : class
    {
        Ensure.NotNull(data);

        if (string.IsNullOrWhiteSpace(data))
        {
            return;
        }

        data.PopulateJson(options, __jsonSerializerOptions);
    }
}

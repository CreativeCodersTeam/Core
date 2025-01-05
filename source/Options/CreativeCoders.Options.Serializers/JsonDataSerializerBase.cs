using System.Text.Json;
using System.Text.Json.Serialization;

namespace CreativeCoders.Options.Serializers;

public abstract class JsonDataSerializerBase
{
    protected static readonly JsonSerializerOptions DefaultJsonSerializerOptions =
        new JsonSerializerOptions
        {
            WriteIndented = true,
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
}

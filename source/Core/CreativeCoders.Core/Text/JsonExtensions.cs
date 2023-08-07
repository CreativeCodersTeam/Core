using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Text;

[PublicAPI]
[ExcludeFromCodeCoverage]
public static class JsonExtensions
{
    public static string ToJson<T>(this T data, JsonSerializerOptions jsonSerializerOptions = null)
    {
        return JsonSerializer.Serialize(data, jsonSerializerOptions);
    }

    public static void ToJson<T>(this T data, Stream utf8Json, JsonSerializerOptions jsonSerializerOptions = null)
    {
        JsonSerializer.Serialize(utf8Json, data, jsonSerializerOptions);
    }
}

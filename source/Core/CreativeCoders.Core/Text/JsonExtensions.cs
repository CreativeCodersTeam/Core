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

    public static T ReadAsJson<T>(this Stream utf8Json, JsonSerializerOptions jsonSerializerOptions = null)
    {
        return JsonSerializer.Deserialize<T>(utf8Json, jsonSerializerOptions);
    }

    public static T ReadAsJson<T>(this string json, JsonSerializerOptions jsonSerializerOptions = null)
    {
        return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
    }
}

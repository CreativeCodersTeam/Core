using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Text;

#nullable enable

[PublicAPI]
[ExcludeFromCodeCoverage]
public static class JsonExtensions
{
    public static string ToJson<T>(this T data, JsonSerializerOptions jsonSerializerOptions = null)
    {
        return JsonSerializer.Serialize(data, jsonSerializerOptions);
    }

    public static void ToJson<T>(this T data, Stream utf8Json,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        JsonSerializer.Serialize(utf8Json, data, jsonSerializerOptions);
    }

    public static T? ReadAsJson<T>(this Stream utf8Json, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        return JsonSerializer.Deserialize<T>(utf8Json, jsonSerializerOptions);
    }

    public static T? ReadAsJson<T>(this string json, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
    }

    public static void PopulateJson<T>(this string json, T obj,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.IsNotNull(json);

        using var jsonDocument = JsonDocument.Parse(json);

        PopulateObjectFromJsonDocument(obj, jsonDocument, jsonSerializerOptions);
    }

    public static void PopulateJson<T>(this Stream utf8Json, T obj,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.IsNotNull(utf8Json);

        using var jsonDocument = JsonDocument.Parse(utf8Json);

        PopulateObjectFromJsonDocument(obj, jsonDocument, jsonSerializerOptions);
    }

    public static async Task PopulateJsonAsync<T>(this Stream utf8Json, T obj,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.IsNotNull(utf8Json);

        using var jsonDocument = await JsonDocument.ParseAsync(utf8Json).ConfigureAwait(false);

        PopulateObjectFromJsonDocument(obj, jsonDocument, jsonSerializerOptions);
    }

    [SuppressMessage("ReSharper", "InvertIf")]
    private static void PopulateObjectFromJsonDocument<T>(T obj, JsonDocument jsonDocument,
        JsonSerializerOptions? jsonSerializerOptions)
    {
        foreach (var property in jsonDocument.RootElement.EnumerateObject())
        {
            var propertyName =
                ConvertPropertyName(jsonSerializerOptions?.PropertyNamingPolicy, property.Name);

            var propertyInfo =
                typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                var propertyValue =
                    JsonSerializer.Deserialize(property.Value.GetRawText(), propertyInfo.PropertyType);

                if (jsonSerializerOptions?.DefaultIgnoreCondition == JsonIgnoreCondition.WhenWritingNull &&
                    propertyValue == null)
                {
                    continue;
                }

                if (jsonSerializerOptions?.NumberHandling == JsonNumberHandling.AllowReadingFromString &&
                    propertyInfo.PropertyType == typeof(int) && propertyValue is string stringValue)
                {
                    propertyValue = int.Parse(stringValue);
                }

                propertyInfo.SetValue(obj, propertyValue);
            }
        }
    }

    private static string ConvertPropertyName(JsonNamingPolicy? propertyNamingPolicy, string propertyName)
    {
        if (propertyNamingPolicy == JsonNamingPolicy.CamelCase)
        {
            return CamelCaseToPascalCase(propertyName);
        }

        if (propertyNamingPolicy == JsonNamingPolicy.KebabCaseUpper)
        {
            return KebabCaseToPascalCase(propertyName);
        }

        return propertyName;
    }

    private static string CamelCaseToPascalCase(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        return char.ToUpperInvariant(text[0]) + text[1..];
    }

    private static string KebabCaseToPascalCase(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        var parts = text.Split('-');

        return parts.Aggregate(string.Empty,
            (current, part) => current + (char.ToUpperInvariant(part[0]) + part[1..].ToLowerInvariant()));
    }
}

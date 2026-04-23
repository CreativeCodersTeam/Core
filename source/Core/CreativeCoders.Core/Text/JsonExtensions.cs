using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Text;

#nullable enable

/// <summary>
/// Provides extension methods for JSON serialization and deserialization using <see cref="JsonSerializer"/>.
/// </summary>
[PublicAPI]
public static class JsonExtensions
{
    /// <summary>
    /// Serializes the object to a JSON string.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="data">The object to serialize.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    /// <returns>A JSON string representation of the object.</returns>
    [ExcludeFromCodeCoverage]
    public static string ToJson<T>(this T data, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        return JsonSerializer.Serialize(data, jsonSerializerOptions);
    }

    /// <summary>
    /// Serializes the object as JSON into the specified UTF-8 stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="data">The object to serialize.</param>
    /// <param name="utf8Json">The UTF-8 stream to write JSON to.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    [ExcludeFromCodeCoverage]
    public static void ToJson<T>(this T data, Stream utf8Json,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        JsonSerializer.Serialize(utf8Json, data, jsonSerializerOptions);
    }

    /// <summary>
    /// Deserializes JSON from a UTF-8 stream into an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="utf8Json">The UTF-8 stream containing JSON data.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    /// <returns>The deserialized object, or <see langword="null"/> if the JSON value is null.</returns>
    [ExcludeFromCodeCoverage]
    public static T? ReadAsJson<T>(this Stream utf8Json, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        return JsonSerializer.Deserialize<T>(utf8Json, jsonSerializerOptions);
    }

    /// <summary>
    /// Deserializes a JSON string into an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    /// <returns>The deserialized object, or <see langword="null"/> if the JSON value is null.</returns>
    [ExcludeFromCodeCoverage]
    public static T? ReadAsJson<T>(this string json, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
    }

    /// <summary>
    /// Populates an existing object's properties from a JSON string.
    /// </summary>
    /// <typeparam name="T">The type of the object to populate.</typeparam>
    /// <param name="json">The JSON string containing property values.</param>
    /// <param name="obj">The existing object to populate.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    public static void PopulateJson<T>(this string json, T obj,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.IsNotNull(json);

        using var jsonDocument = JsonDocument.Parse(json,
            new JsonDocumentOptions
                { AllowTrailingCommas = jsonSerializerOptions?.AllowTrailingCommas ?? false });

        PopulateObjectFromJsonDocument(obj, jsonDocument, jsonSerializerOptions);
    }

    /// <summary>
    /// Populates an existing object's properties from a UTF-8 JSON stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to populate.</typeparam>
    /// <param name="utf8Json">The UTF-8 stream containing JSON data.</param>
    /// <param name="obj">The existing object to populate.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    public static void PopulateJson<T>(this Stream utf8Json, T obj,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.IsNotNull(utf8Json);

        using var jsonDocument = JsonDocument.Parse(utf8Json, new JsonDocumentOptions
            { AllowTrailingCommas = jsonSerializerOptions?.AllowTrailingCommas ?? false });

        PopulateObjectFromJsonDocument(obj, jsonDocument, jsonSerializerOptions);
    }

    /// <summary>
    /// Asynchronously populates an existing object's properties from a UTF-8 JSON stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to populate.</typeparam>
    /// <param name="utf8Json">The UTF-8 stream containing JSON data.</param>
    /// <param name="obj">The existing object to populate.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    /// <returns>A task representing the asynchronous populate operation.</returns>
    public static async Task PopulateJsonAsync<T>(this Stream utf8Json, T obj,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.IsNotNull(utf8Json);

        using var jsonDocument = await JsonDocument.ParseAsync(utf8Json, new JsonDocumentOptions
                { AllowTrailingCommas = jsonSerializerOptions?.AllowTrailingCommas ?? false })
            .ConfigureAwait(false);

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

            var propertyInfo = GetPropertyInfo<T>(propertyName,
                jsonSerializerOptions?.PropertyNameCaseInsensitive ?? false);

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

    private static PropertyInfo? GetPropertyInfo<T>(string propertyName, bool caseInsensitive)
    {
        return caseInsensitive
            ? typeof(T).GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
            : typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
    }

    private static string ConvertPropertyName(JsonNamingPolicy? propertyNamingPolicy, string propertyName)
    {
        if (propertyNamingPolicy == JsonNamingPolicy.CamelCase)
        {
            return propertyName.CamelCaseToPascalCase();
        }

        if (propertyNamingPolicy == JsonNamingPolicy.KebabCaseUpper ||
            propertyNamingPolicy == JsonNamingPolicy.KebabCaseLower)
        {
            return propertyName.KebabCaseToPascalCase();
        }

        if (propertyNamingPolicy == JsonNamingPolicy.SnakeCaseUpper ||
            propertyNamingPolicy == JsonNamingPolicy.SnakeCaseLower)
        {
            return propertyName.SnakeCaseToPascalCase();
        }

        return propertyName;
    }
}

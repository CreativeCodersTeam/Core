using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Text.Json;

#nullable enable

/// <summary>
/// Defines methods for serializing, deserializing, and populating objects using JSON.
/// </summary>
[PublicAPI]
public interface IJsonSerializer
{
    /// <summary>
    /// Serializes the specified object to a JSON string.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="data">The object to serialize.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    /// <returns>A JSON string representation of the object.</returns>
    string Serialize<T>(T data, JsonSerializerOptions? jsonSerializerOptions = null);

    /// <summary>
    /// Serializes the specified object as JSON into a UTF-8 stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="utf8Json">The UTF-8 stream to write JSON to.</param>
    /// <param name="data">The object to serialize.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    void Serialize<T>(Stream utf8Json, T data, JsonSerializerOptions? jsonSerializerOptions = null);

    /// <summary>
    /// Asynchronously serializes the specified object as JSON into a UTF-8 stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="utf8Json">The UTF-8 stream to write JSON to.</param>
    /// <param name="data">The object to serialize.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    /// <returns>A task representing the asynchronous serialize operation.</returns>
    Task SerializeAsync<T>(Stream utf8Json, T data, JsonSerializerOptions? jsonSerializerOptions = null);

    /// <summary>
    /// Deserializes a JSON string into an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    /// <returns>The deserialized object, or <see langword="null"/> if the JSON value is null.</returns>
    T? Deserialize<T>(string json, JsonSerializerOptions? jsonSerializerOptions = null);

    /// <summary>
    /// Deserializes JSON from a UTF-8 stream into an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="utf8Json">The UTF-8 stream containing JSON data.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    /// <returns>The deserialized object, or <see langword="null"/> if the JSON value is null.</returns>
    T? Deserialize<T>(Stream utf8Json, JsonSerializerOptions? jsonSerializerOptions = null);

    /// <summary>
    /// Asynchronously deserializes JSON from a UTF-8 stream into an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="utf8Json">The UTF-8 stream containing JSON data.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    /// <returns>A value task containing the deserialized object, or <see langword="null"/> if the JSON value is null.</returns>
    ValueTask<T?> DeserializeAsync<T>(Stream utf8Json, JsonSerializerOptions? jsonSerializerOptions = null);

    /// <summary>
    /// Populates an existing object's properties from a JSON string.
    /// </summary>
    /// <typeparam name="T">The type of the object to populate.</typeparam>
    /// <param name="json">The JSON string containing property values.</param>
    /// <param name="obj">The existing object to populate.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    void Populate<T>(string json, T obj, JsonSerializerOptions? jsonSerializerOptions = null);

    /// <summary>
    /// Populates an existing object's properties from a UTF-8 JSON stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to populate.</typeparam>
    /// <param name="utf8Json">The UTF-8 stream containing JSON data.</param>
    /// <param name="obj">The existing object to populate.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    void Populate<T>(Stream utf8Json, T obj, JsonSerializerOptions? jsonSerializerOptions = null);

    /// <summary>
    /// Asynchronously populates an existing object's properties from a UTF-8 JSON stream.
    /// </summary>
    /// <typeparam name="T">The type of the object to populate.</typeparam>
    /// <param name="utf8Json">The UTF-8 stream containing JSON data.</param>
    /// <param name="obj">The existing object to populate.</param>
    /// <param name="jsonSerializerOptions">Optional serializer options.</param>
    /// <returns>A task representing the asynchronous populate operation.</returns>
    Task PopulateAsync<T>(Stream utf8Json, T obj, JsonSerializerOptions? jsonSerializerOptions = null);
}

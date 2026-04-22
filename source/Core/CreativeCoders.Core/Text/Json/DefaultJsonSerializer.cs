using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Text.Json;

#nullable enable

/// <summary>
/// Provides a default implementation of <see cref="IJsonSerializer"/> using <see cref="JsonSerializer"/>.
/// </summary>
[ExcludeFromCodeCoverage]
[PublicAPI]
[SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
public class DefaultJsonSerializer : IJsonSerializer
{
    /// <inheritdoc/>
    public string Serialize<T>(T data, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.NotNull(data);

        return JsonSerializer.Serialize(data, jsonSerializerOptions);
    }

    /// <inheritdoc/>
    public void Serialize<T>(Stream utf8Json, T data, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.NotNull(utf8Json);
        Ensure.NotNull(data);

        JsonSerializer.Serialize(utf8Json, data, jsonSerializerOptions);
    }

    /// <inheritdoc/>
    public Task SerializeAsync<T>(Stream utf8Json, T data,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.NotNull(utf8Json);
        Ensure.NotNull(data);

        return JsonSerializer.SerializeAsync(utf8Json, data, jsonSerializerOptions);
    }

    /// <inheritdoc/>
    public T? Deserialize<T>(string json, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.NotNull(json);

        return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
    }

    /// <inheritdoc/>
    public T? Deserialize<T>(Stream utf8Json, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.NotNull(utf8Json);

        return JsonSerializer.Deserialize<T>(utf8Json, jsonSerializerOptions);
    }

    /// <inheritdoc/>
    public ValueTask<T?> DeserializeAsync<T>(Stream utf8Json,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.NotNull(utf8Json);

        return JsonSerializer.DeserializeAsync<T>(utf8Json, jsonSerializerOptions);
    }

    /// <inheritdoc/>
    public void Populate<T>(string json, T obj, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        JsonExtensions.PopulateJson(json, obj, jsonSerializerOptions);
    }

    /// <inheritdoc/>
    public void Populate<T>(Stream utf8Json, T obj, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        JsonExtensions.PopulateJson(utf8Json, obj, jsonSerializerOptions);
    }

    /// <inheritdoc/>
    public Task PopulateAsync<T>(Stream utf8Json, T obj,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        return JsonExtensions.PopulateJsonAsync(utf8Json, obj, jsonSerializerOptions);
    }
}

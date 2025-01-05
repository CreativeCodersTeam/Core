using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Text.Json;

#nullable enable

[ExcludeFromCodeCoverage]
[PublicAPI]
[SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
public class DefaultJsonSerializer : IJsonSerializer
{
    public string Serialize<T>(T data, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.NotNull(data);

        return JsonSerializer.Serialize(data, jsonSerializerOptions);
    }

    public void Serialize<T>(Stream utf8Json, T data, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.NotNull(utf8Json);
        Ensure.NotNull(data);

        JsonSerializer.Serialize(utf8Json, data, jsonSerializerOptions);
    }

    public Task SerializeAsync<T>(Stream utf8Json, T data,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.NotNull(utf8Json);
        Ensure.NotNull(data);

        return JsonSerializer.SerializeAsync(utf8Json, data, jsonSerializerOptions);
    }

    public T? Deserialize<T>(string json, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.NotNull(json);

        return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
    }

    public T? Deserialize<T>(Stream utf8Json, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.NotNull(utf8Json);

        return JsonSerializer.Deserialize<T>(utf8Json, jsonSerializerOptions);
    }

    public ValueTask<T?> DeserializeAsync<T>(Stream utf8Json,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        Ensure.NotNull(utf8Json);

        return JsonSerializer.DeserializeAsync<T>(utf8Json, jsonSerializerOptions);
    }

    public void Populate<T>(string json, T obj, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        JsonExtensions.PopulateJson(json, obj, jsonSerializerOptions);
    }

    public void Populate<T>(Stream utf8Json, T obj, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        JsonExtensions.PopulateJson(utf8Json, obj, jsonSerializerOptions);
    }

    public Task PopulateAsync<T>(Stream utf8Json, T obj,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        return JsonExtensions.PopulateJsonAsync(utf8Json, obj, jsonSerializerOptions);
    }
}

using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Text.Json;

#nullable enable

[PublicAPI]
public interface IJsonSerializer
{
    string Serialize<T>(T data, JsonSerializerOptions? jsonSerializerOptions = null);
    void Serialize<T>(Stream utf8Json, T data, JsonSerializerOptions? jsonSerializerOptions = null);
    Task SerializeAsync<T>(Stream utf8Json, T data, JsonSerializerOptions? jsonSerializerOptions = null);
    T? Deserialize<T>(string json, JsonSerializerOptions? jsonSerializerOptions = null);
    T? Deserialize<T>(Stream utf8Json, JsonSerializerOptions? jsonSerializerOptions = null);
    ValueTask<T?> DeserializeAsync<T>(Stream utf8Json, JsonSerializerOptions? jsonSerializerOptions = null);
    void Populate<T>(string json, T obj, JsonSerializerOptions? jsonSerializerOptions = null);
    void Populate<T>(Stream utf8Json, T obj, JsonSerializerOptions? jsonSerializerOptions = null);
    Task PopulateAsync<T>(Stream utf8Json, T obj, JsonSerializerOptions? jsonSerializerOptions = null);
}

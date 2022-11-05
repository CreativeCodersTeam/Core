using System.Text.Json;

namespace CreativeCoders.Net.WebApi.Serialization.Json;

public class JsonDataDeserializer : IDataDeserializer
{
    public T Deserialize<T>(string data)
    {
        return JsonSerializer.Deserialize<T>(data);
    }
}

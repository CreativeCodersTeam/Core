using System.Text.Json;

namespace CreativeCoders.Net.WebApi.Serialization.Json;

public class JsonDataSerializer : IDataSerializer
{
    public string Serialize<T>(T data)
    {
        return JsonSerializer.Serialize(data);
    }
}

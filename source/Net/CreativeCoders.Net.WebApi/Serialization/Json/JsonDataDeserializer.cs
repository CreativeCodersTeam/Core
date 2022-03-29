using Newtonsoft.Json;

namespace CreativeCoders.Net.WebApi.Serialization.Json;

public class JsonDataDeserializer : IDataDeserializer
{
    public T Deserialize<T>(string data)
    {
        return JsonConvert.DeserializeObject<T>(data);
    }
}
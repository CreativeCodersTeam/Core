using Newtonsoft.Json;

namespace CreativeCoders.Net.WebApi.Serialization.Json;

public class JsonDataSerializer : IDataSerializer
{
    public string Serialize<T>(T data)
    {
        return JsonConvert.SerializeObject(data);
    }
}

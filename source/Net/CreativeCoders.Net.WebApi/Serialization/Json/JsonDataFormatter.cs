namespace CreativeCoders.Net.WebApi.Serialization.Json;

public class JsonDataFormatter : IDataFormatter
{
    public IDataDeserializer GetDeserializer()
    {
        return new JsonDataDeserializer();
    }

    public IDataSerializer GetSerializer()
    {
        return new JsonDataSerializer();
    }

    public string ContentMediaType => ContentMediaTypes.Application.Json;

    public string Name => DataFormat.Json;
}

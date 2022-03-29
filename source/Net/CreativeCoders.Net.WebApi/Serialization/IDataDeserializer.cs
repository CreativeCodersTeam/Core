namespace CreativeCoders.Net.WebApi.Serialization;

public interface IDataDeserializer
{
    T Deserialize<T>(string data);
}
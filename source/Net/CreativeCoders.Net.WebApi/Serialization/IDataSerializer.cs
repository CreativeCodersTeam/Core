namespace CreativeCoders.Net.WebApi.Serialization;

public interface IDataSerializer
{
    string Serialize<T>(T data);
}
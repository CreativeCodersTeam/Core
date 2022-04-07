namespace CreativeCoders.Net.WebApi.Serialization;

public interface IDataFormatter
{
    IDataDeserializer GetDeserializer();

    IDataSerializer GetSerializer();

    string ContentMediaType { get; }

    string Name { get; }
}

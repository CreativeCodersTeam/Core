using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Serialization;

[PublicAPI]
public interface IDataFormatter
{
    IDataDeserializer GetDeserializer();

    IDataSerializer GetSerializer();

    string ContentMediaType { get; }

    string Name { get; }
}

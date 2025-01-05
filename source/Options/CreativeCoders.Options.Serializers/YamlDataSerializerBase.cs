using YamlDotNet.Serialization;

namespace CreativeCoders.Options.Serializers;

public abstract class YamlDataSerializerBase
{
    protected static readonly ISerializer YamlSerializer = new SerializerBuilder().Build();
}

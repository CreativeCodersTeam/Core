using CreativeCoders.Net.XmlRpc.Model.Values;

namespace CreativeCoders.Net.XmlRpc.Reader.Values;

public class BooleanValueReader : ValueReaderBase
{
    public BooleanValueReader() :
        base(new[] {XmlRpcTags.Boolean}, value => new BooleanValue(value == "1")) { }
}

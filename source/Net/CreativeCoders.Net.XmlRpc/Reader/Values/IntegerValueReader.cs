using CreativeCoders.Net.XmlRpc.Model.Values;

namespace CreativeCoders.Net.XmlRpc.Reader.Values;

public class IntegerValueReader : ValueReaderBase
{
    public IntegerValueReader() : base(new[] {XmlRpcTags.I4, XmlRpcTags.Int},
        value => new IntegerValue(int.Parse(value))) { }
}

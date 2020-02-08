using CreativeCoders.Net.XmlRpc.Model.Values;

namespace CreativeCoders.Net.XmlRpc.Reader.Values
{
    public class StringValueReader : ValueReaderBase
    {
        public StringValueReader() : base(new []{ XmlRpcTags.Value, XmlRpcTags.String}, value => new StringValue(value))
        {
        }
    }
}
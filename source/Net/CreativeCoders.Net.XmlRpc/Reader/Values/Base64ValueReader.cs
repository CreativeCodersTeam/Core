using System.Text;
using CreativeCoders.Net.XmlRpc.Model.Values;

namespace CreativeCoders.Net.XmlRpc.Reader.Values;

public class Base64ValueReader : ValueReaderBase
{
    public Base64ValueReader(Encoding encoding) : base(new[] {XmlRpcTags.Base64},
        value => new Base64Value(value, encoding)) { }
}

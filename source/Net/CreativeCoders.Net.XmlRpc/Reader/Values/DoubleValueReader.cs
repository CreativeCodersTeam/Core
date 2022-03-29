using System.Globalization;
using CreativeCoders.Net.XmlRpc.Model.Values;

namespace CreativeCoders.Net.XmlRpc.Reader.Values;

public class DoubleValueReader : ValueReaderBase
{
    public DoubleValueReader() : base(new[] {XmlRpcTags.Double},
        value => new DoubleValue(double.Parse(value, NumberFormatInfo.InvariantInfo)))
    {
    }
}
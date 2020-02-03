using System.Globalization;

namespace CreativeCoders.Net.XmlRpc.Writer.Values
{
    public class DoubleValueWriter : ValueWriterBase<double>
    {
        public DoubleValueWriter() : base(XmlRpcTags.Double, value => value.Value.ToString(NumberFormatInfo.InvariantInfo))
        {
        }
    }
}
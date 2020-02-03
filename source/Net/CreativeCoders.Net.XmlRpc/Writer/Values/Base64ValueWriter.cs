using System;

namespace CreativeCoders.Net.XmlRpc.Writer.Values
{
    public class Base64ValueWriter : ValueWriterBase<byte[]>
    {
        public Base64ValueWriter() : base(XmlRpcTags.Base64, value => Convert.ToBase64String(value.Value))
        {
        }
    }
}
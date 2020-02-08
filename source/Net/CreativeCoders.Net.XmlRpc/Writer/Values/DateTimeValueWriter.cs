using System;

namespace CreativeCoders.Net.XmlRpc.Writer.Values
{
    public class DateTimeValueWriter : ValueWriterBase<DateTime>
    {
        public DateTimeValueWriter() : base(XmlRpcTags.DateTime, value => value.Value.ToString(XmlRpcConstants.DateTimeFormat))
        {
        }
    }
}
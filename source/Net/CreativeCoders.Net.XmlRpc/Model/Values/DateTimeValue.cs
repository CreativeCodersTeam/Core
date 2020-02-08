using System;

namespace CreativeCoders.Net.XmlRpc.Model.Values
{
    public class DateTimeValue : XmlRpcValue<DateTime>
    {
        public DateTimeValue(DateTime value) : base(value)
        {
        }
    }
}
using System;
using System.Globalization;
using CreativeCoders.Net.XmlRpc.Model.Values;

namespace CreativeCoders.Net.XmlRpc.Reader.Values;

public class DateTimeValueReader : ValueReaderBase
{
    public DateTimeValueReader() : base(new[] {XmlRpcTags.DateTime},
        value => new DateTimeValue(
            DateTime.ParseExact(value, XmlRpcConstants.DateTimeFormat, DateTimeFormatInfo.InvariantInfo)))
    {
    }
}
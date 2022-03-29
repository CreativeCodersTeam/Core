using System;
using System.Text;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Model.Values;

[PublicAPI]
public class Base64Value : XmlRpcValue<byte[]>
{
    private readonly Encoding _encoding;

    public Base64Value(string value, Encoding encoding) : base(GetEncodedBytes(value))
    {
        _encoding = encoding;
    }

    public Base64Value(byte[] value, Encoding encoding) : base(value)
    {
        _encoding = encoding;
    }

    private static byte[] GetEncodedBytes(string value)
    {
        return Convert.FromBase64String(value);
    }

    public string GetEncodedString(Encoding encoding)
    {
        return encoding.GetString(Value);
    }

    public string GetEncodedString()
    {
        return GetEncodedString(_encoding);
    }
}

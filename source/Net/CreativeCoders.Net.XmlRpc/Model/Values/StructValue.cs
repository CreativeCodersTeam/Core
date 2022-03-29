using System.Collections.Generic;

namespace CreativeCoders.Net.XmlRpc.Model.Values;

public class StructValue : XmlRpcValue<IDictionary<string, XmlRpcValue>>
{
    public StructValue() : this(new Dictionary<string, XmlRpcValue>())
    {
    }

    public StructValue(IDictionary<string, XmlRpcValue> value) : base(value)
    {
    }
}
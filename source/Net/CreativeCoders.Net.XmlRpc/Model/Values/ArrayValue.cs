using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Net.XmlRpc.Model.Values;

public class ArrayValue : XmlRpcValue<IEnumerable<XmlRpcValue>>
{
    public ArrayValue(IEnumerable<XmlRpcValue> value) :
        base(value?.ToArray() ?? Array.Empty<XmlRpcValue>()) { }
}

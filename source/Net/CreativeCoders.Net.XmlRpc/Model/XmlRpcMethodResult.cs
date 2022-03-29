using System.Collections.Generic;

namespace CreativeCoders.Net.XmlRpc.Model;

public class XmlRpcMethodResult
{
    public XmlRpcMethodResult(params XmlRpcValue[] values)
    {
        Values = values;
        IsFaulted = false;
    }

    public XmlRpcMethodResult(int faultCode, string faultString) : this()
    {
        IsFaulted = true;
        FaultCode = faultCode;
        FaultString = faultString;
    }

    public IEnumerable<XmlRpcValue> Values { get; }

    public bool IsFaulted { get; }

    public int FaultCode { get; }

    public string FaultString { get; }
}

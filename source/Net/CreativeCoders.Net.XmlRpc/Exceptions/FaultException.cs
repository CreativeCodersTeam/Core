using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Exceptions;

[PublicAPI]
public class FaultException : XmlRpcException
{
    public FaultException(int faultCode, string faultMessage)
        : base($"Xml rpc call result is faulted. FaultCode: {faultCode} FaultString: '{faultMessage}'")
    {
        FaultCode = faultCode;
        FaultMessage = faultMessage;
    }

    public int FaultCode { get; }

    public string FaultMessage { get; }
}
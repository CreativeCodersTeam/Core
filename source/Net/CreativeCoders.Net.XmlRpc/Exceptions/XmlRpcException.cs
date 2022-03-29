using System;

namespace CreativeCoders.Net.XmlRpc.Exceptions;

public abstract class XmlRpcException : Exception
{
    protected XmlRpcException(string message) : base(message)
    {
    }
}
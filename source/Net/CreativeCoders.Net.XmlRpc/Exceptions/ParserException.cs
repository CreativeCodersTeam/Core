using System;

namespace CreativeCoders.Net.XmlRpc.Exceptions
{
    public class ParserException : XmlRpcException
    {
        public ParserException(string message) : base(message)
        {
        }

        public ParserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
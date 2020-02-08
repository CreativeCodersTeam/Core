using System;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Exceptions
{
    public class ReturnTypeNotSupportedException : XmlRpcException
    {
        public ReturnTypeNotSupportedException(ApiMethodReturnType returnType)
            : base($"Return type '{returnType}' is not supported")
        {
        }

        public ReturnTypeNotSupportedException(ApiMethodReturnType returnType, Exception innerException)
            : base($"Return type '{returnType}' is not supported", innerException)
        {
        }
    }
}
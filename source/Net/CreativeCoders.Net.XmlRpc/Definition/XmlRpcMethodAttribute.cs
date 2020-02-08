using System;

namespace CreativeCoders.Net.XmlRpc.Definition
{
    public class XmlRpcMethodAttribute : Attribute
    {
        public XmlRpcMethodAttribute() : this(string.Empty)
        {
        }

        public XmlRpcMethodAttribute(string methodName)
        {
            MethodName = methodName;
        }

        public string MethodName { get; }

        public Type ExceptionHandler { get; set; }

        public object DefaultResult { get; set; }
    }
}
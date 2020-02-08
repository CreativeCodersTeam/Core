using System;
using System.Reflection;
using CreativeCoders.Net.XmlRpc.Definition;

namespace CreativeCoders.Net.XmlRpc.Proxy.Specification
{
    public class ApiMethodInfo
    {
        public MethodInfo Method { get; set; }

        public ApiMethodReturnType ReturnType { get; set; }

        public string MethodName { get; set; }

        public Type ValueType { get; set; }

        public IMethodExceptionHandler ExceptionHandler { get; set; }

        public object DefaultResult { get; set; }
    }
}
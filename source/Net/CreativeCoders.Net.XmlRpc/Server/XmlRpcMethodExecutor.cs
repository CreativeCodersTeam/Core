using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.Net.XmlRpc.Definition;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;

namespace CreativeCoders.Net.XmlRpc.Server
{
    public class XmlRpcMethodExecutor
    {
        private readonly IXmlRpcServerMethods _xmlRpcServerMethods;

        public XmlRpcMethodExecutor(IXmlRpcServerMethods xmlRpcServerMethods)
        {
            Ensure.IsNotNull(xmlRpcServerMethods, nameof(xmlRpcServerMethods));

            _xmlRpcServerMethods = xmlRpcServerMethods;
            _xmlRpcServerMethods.RegisterMethods("system", this);
        }

        public object Invoke(XmlRpcMethodCall methodCall)
        {
            Ensure.IsNotNull(methodCall, nameof(methodCall));

            var methodRegistration = _xmlRpcServerMethods.GetMethod(methodCall.Name);

            if (methodRegistration == null)
            {
                throw new MissingMethodException();
            }

            var parameters = methodCall.Parameters.Select(p => p.Data).ToArray();

            return methodRegistration.Method.Invoke(methodRegistration.Target,
                methodRegistration.Method.GetParameters().Any()
                    ? parameters
                    : new object[0]);
        }

        [XmlRpcMethod("listMethods")]
        public IEnumerable<string> ListMethods()
        {
            return _xmlRpcServerMethods.Methods.Select(x => new StringValue(x.MethodName).Value);
        }
    }
}
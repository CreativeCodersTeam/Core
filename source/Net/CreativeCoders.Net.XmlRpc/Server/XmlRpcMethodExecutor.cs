using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Threading;
using CreativeCoders.Net.XmlRpc.Definition;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Model.Values.Converters;

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

        public async Task<object> Invoke(XmlRpcMethodCall methodCall)
        {
            Ensure.IsNotNull(methodCall, nameof(methodCall));

            var methodRegistration = _xmlRpcServerMethods.GetMethod(methodCall.Name);

            if (methodRegistration == null)
            {
                throw new MissingMethodException();
            }
            
            var parameters = GetParameters(methodRegistration.Method.GetParameters(), methodCall.Parameters.ToArray());

            if (!methodRegistration.Method.ReturnType.IsGenericType ||
                methodRegistration.Method.ReturnType.GetGenericTypeDefinition() != typeof(Task<>))
            {
                return methodRegistration.Method.Invoke(methodRegistration.Target, parameters);
            }

            var task = (Task) methodRegistration.Method.Invoke(methodRegistration.Target, parameters);

            return await task.ToTask<object>();
        }

        private static object[] GetParameters(IReadOnlyList<ParameterInfo> parameterInfos, IReadOnlyCollection<XmlRpcValue> parameters)
        {
            if (!parameterInfos.Any())
            {
                return new object[0];
            }

            if (parameters.Count != parameterInfos.Count)
            {
                throw new TargetParameterCountException(
                    $"Xml Rpc method parameter count mismatch. Method parameter count ({parameterInfos.Count}) != calling parameter count ({parameters.Count})");
            }
            
            var converter = new XmlRpcValueToDataConverter();

            return parameters
                .Select((x, index) => converter.Convert(x, parameterInfos[index].ParameterType))
                .ToArray();
        }

        [XmlRpcMethod("listMethods")]
        public IEnumerable<string> ListMethods()
        {
            return _xmlRpcServerMethods.Methods.Select(x => new StringValue(x.MethodName).Value);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CreativeCoders.Net.XmlRpc.Definition;
using CreativeCoders.Net.XmlRpc.Exceptions;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy.Analyzing
{
    public class ApiMethodAnalyzer
    {
        private readonly MethodInfo _methodInfo;
        
        private readonly IMethodExceptionHandler _globalExceptionHandler;

        private readonly XmlRpcMethodAttribute _methodAttr;

        public ApiMethodAnalyzer(MethodInfo methodInfo, IMethodExceptionHandler globalExceptionHandler)
        {
            _methodInfo = methodInfo;
            _globalExceptionHandler = globalExceptionHandler;
            _methodAttr = _methodInfo.GetCustomAttribute(typeof(XmlRpcMethodAttribute)) as XmlRpcMethodAttribute;
        }
        
        public ApiMethodInfo Analyze()
        {
            var apiMethodInfo = new ApiMethodInfo
            {
                Method = _methodInfo,
                MethodName = GetMethodName(),
                DefaultResult = _methodAttr.DefaultResult,
                ExceptionHandler = GetExceptionHandler()
            };

            SetMethodReturnType(apiMethodInfo);

            return apiMethodInfo;
        }

        private IMethodExceptionHandler GetExceptionHandler()
        {
            return _methodAttr.ExceptionHandler != null
                ? Activator.CreateInstance(_methodAttr.ExceptionHandler) as IMethodExceptionHandler
                : _globalExceptionHandler;
        }

        private string GetMethodName()
        {
            return string.IsNullOrEmpty(_methodAttr?.MethodName) ? _methodInfo.Name : _methodAttr.MethodName;
        }

        private void SetMethodReturnType(ApiMethodInfo apiMethodInfo)
        {
            if (_methodInfo.ReturnType.IsGenericType &&
                _methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var genericArgument = _methodInfo.ReturnType.GetGenericArguments().First();

                if (genericArgument.IsGenericParameter)
                {
                    apiMethodInfo.ReturnType = ApiMethodReturnType.GenericValue;
                    return;
                }

                if (typeof(XmlRpcValue).IsAssignableFrom(genericArgument))
                {
                    apiMethodInfo.ReturnType = ApiMethodReturnType.XmlRpcValue;
                    apiMethodInfo.ValueType = genericArgument;
                    return;
                }

                if (genericArgument == typeof(string) || genericArgument.IsValueType)
                {
                    apiMethodInfo.ReturnType = ApiMethodReturnType.Value;
                    apiMethodInfo.ValueType = genericArgument;
                    return;
                }

                if (genericArgument.IsGenericType && genericArgument.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                {
                    apiMethodInfo.ReturnType = ApiMethodReturnType.Dictionary;
                    apiMethodInfo.ValueType = genericArgument.GetGenericArguments().Last();
                    return;
                }

                if (genericArgument.IsGenericType && genericArgument.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    apiMethodInfo.ReturnType = ApiMethodReturnType.Enumerable;
                    apiMethodInfo.ValueType = genericArgument.GetGenericArguments().First();
                    return;
                }

                if (genericArgument.IsClass || genericArgument.IsInterface)
                {
                    apiMethodInfo.ReturnType = ApiMethodReturnType.ObjectValue;
                    apiMethodInfo.ValueType = genericArgument;
                    return;
                }
            }

            if (_methodInfo.ReturnType != typeof(Task))
            {
                throw new WrongReturnTypeException(_methodInfo);
            }
            
            apiMethodInfo.ReturnType = ApiMethodReturnType.Void;
        }
    }
}
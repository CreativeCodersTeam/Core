using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core.Reflection;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Model.Values.Converters;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy.Execution.Requests
{
    public class EnumerableRequestHandler : XmlRpcRequestHandlerBase
    {
        public EnumerableRequestHandler() : base(ApiMethodReturnType.Enumerable)
        {
        }

        public override object HandleRequest(RequestData requestData)
        {
            return this.ExecuteGenericMethod<object>(nameof(GetEnumerableAsync), new[] { requestData.ValueType }, requestData);
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private async Task<IEnumerable<T>> GetEnumerableAsync<T>(RequestData requestData)
        {
            return await GetValueWithExceptionHandlingAsync(() => GetEnumerableInternalAsync<T>(requestData), requestData).ConfigureAwait(false);
        }
        
        // ReSharper disable once MemberCanBeMadeStatic.Local
        private async Task<IEnumerable<T>> GetEnumerableInternalAsync<T>(RequestData requestData)
        {
            var xmlRpcResponse = await requestData.Client.InvokeExAsync(requestData.MethodName, requestData.Arguments).ConfigureAwait(false);

            if (!(xmlRpcResponse.Results.First().Values.First() is ArrayValue xmlRpcValue))
            {
                throw new InvalidOperationException("Xml rpc result must be an array");
            }

            var xmlRpcValueToDataConverter = new XmlRpcValueToDataConverter();

            var value = xmlRpcValue.Value.Select(x => xmlRpcValueToDataConverter.Convert<T>(x));

            return value;
        }
    }
}
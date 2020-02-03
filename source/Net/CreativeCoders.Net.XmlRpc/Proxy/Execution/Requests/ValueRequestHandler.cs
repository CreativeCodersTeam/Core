using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core.Reflection;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy.Execution.Requests
{
    public class ValueRequestHandler : XmlRpcRequestHandlerBase
    {
        public ValueRequestHandler() : base(ApiMethodReturnType.Value)
        {
        }

        public override object HandleRequest(RequestData requestData)
        {
            return this.ExecuteGenericMethod<object>(nameof(GetValueAsync), new[] {requestData.ValueType}, requestData);
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private async Task<T> GetValueAsync<T>(RequestData requestData)
        {
            return await GetValueWithExceptionHandlingAsync(() => GetValueInternalAsync<T>(requestData), requestData).ConfigureAwait(false);
        }
        
        // ReSharper disable once MemberCanBeMadeStatic.Local
        private async Task<T> GetValueInternalAsync<T>(RequestData requestData)
        {
            var response = await requestData.Client.InvokeAsync(requestData.MethodName, requestData.Arguments).ConfigureAwait(false);

            return (T) response.Results.First().Values.First().Data;
        }
    }
}
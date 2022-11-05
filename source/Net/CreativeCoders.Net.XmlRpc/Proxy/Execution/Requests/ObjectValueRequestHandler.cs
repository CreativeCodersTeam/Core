using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core.Reflection;
using CreativeCoders.Net.XmlRpc.Model.Values.Converters;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy.Execution.Requests;

public class ObjectValueRequestHandler : XmlRpcRequestHandlerBase
{
    public ObjectValueRequestHandler() : base(ApiMethodReturnType.ObjectValue) { }

    public override object HandleRequest(RequestData requestData)
    {
        return this.ExecuteGenericMethod<object>(nameof(GetObjectValueAsync), new[] {requestData.ValueType},
            requestData);
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private async Task<T> GetObjectValueAsync<T>(RequestData requestData)
    {
        return await GetValueWithExceptionHandlingAsync(() => GetObjectValueInternalAsync<T>(requestData),
            requestData).ConfigureAwait(false);
    }

    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
    [SuppressMessage("Performance", "CA1822")]
    private async Task<T> GetObjectValueInternalAsync<T>(RequestData requestData)
    {
        var xmlRpcResponse = await requestData.Client
            .InvokeExAsync(requestData.MethodName, requestData.Arguments).ConfigureAwait(false);

        var xmlRpcValue = xmlRpcResponse.Results.First().Values.First();

        var xmlRpcValueToDataConverter = new XmlRpcValueToDataConverter();

        var value = xmlRpcValueToDataConverter.Convert<T>(xmlRpcValue);

        return value;
    }
}

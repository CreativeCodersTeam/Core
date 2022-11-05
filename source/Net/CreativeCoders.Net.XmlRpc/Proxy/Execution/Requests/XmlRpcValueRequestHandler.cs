using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core.Reflection;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy.Execution.Requests;

public class XmlRpcValueRequestHandler : XmlRpcRequestHandlerBase
{
    public XmlRpcValueRequestHandler() : base(ApiMethodReturnType.XmlRpcValue) { }

    public override object HandleRequest(RequestData requestData)
    {
        return this.ExecuteGenericMethod<object>(nameof(GetXmlRpcValueAsync), new[] {requestData.ValueType},
            requestData);
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private async Task<T> GetXmlRpcValueAsync<T>(RequestData requestData)
        where T : XmlRpcValue
    {
        return await GetValueWithExceptionHandlingAsync(() => GetXmlRpcValueInternalAsync<T>(requestData),
            requestData).ConfigureAwait(false);
    }

    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
    [SuppressMessage("Performance", "CA1822")]
    private async Task<T> GetXmlRpcValueInternalAsync<T>(RequestData requestData)
        where T : XmlRpcValue
    {
        var response = await requestData.Client.InvokeAsync(requestData.MethodName, requestData.Arguments)
            .ConfigureAwait(false);

        return (T) response.Results.First().Values.First();
    }
}

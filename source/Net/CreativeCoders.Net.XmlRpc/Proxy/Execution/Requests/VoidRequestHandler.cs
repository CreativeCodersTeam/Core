using System.Threading.Tasks;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy.Execution.Requests;

public class VoidRequestHandler : XmlRpcRequestHandlerBase
{
    public VoidRequestHandler() : base(ApiMethodReturnType.Void)
    {
    }

    public override object HandleRequest(RequestData requestData)
    {
        return ExecuteAsync(requestData);
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private async Task ExecuteAsync(RequestData requestData)
    {
        await ExecuteWithExceptionHandlingAsync(
            () => requestData.Client.ExecuteAsync(requestData.MethodName, requestData.Arguments), requestData).ConfigureAwait(false);
    }
}
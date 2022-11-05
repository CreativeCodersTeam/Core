using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CreativeCoders.Net.XmlRpc.Proxy.Specification;

namespace CreativeCoders.Net.XmlRpc.Proxy.Execution.Requests;

public class VoidRequestHandler : XmlRpcRequestHandlerBase
{
    public VoidRequestHandler() : base(ApiMethodReturnType.Void) { }

    public override object HandleRequest(RequestData requestData)
    {
        return ExecuteAsync(requestData);
    }

    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
    [SuppressMessage("Performance", "CA1822")]
    private async Task ExecuteAsync(RequestData requestData)
    {
        await ExecuteWithExceptionHandlingAsync(
                () => requestData.Client.ExecuteAsync(requestData.MethodName, requestData.Arguments),
                requestData)
            .ConfigureAwait(false);
    }
}

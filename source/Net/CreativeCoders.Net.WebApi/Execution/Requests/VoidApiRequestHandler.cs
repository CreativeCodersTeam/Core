using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Execution.Requests;

public class VoidApiRequestHandler : ApiRequestHandlerBase
{
    public VoidApiRequestHandler(HttpClient httpClient) : base(ApiMethodReturnType.Void, httpClient) { }

    public override object SendRequest(RequestData requestData)
    {
        return RequestVoidAsync(requestData);
    }

    private async Task RequestVoidAsync(RequestData requestData)
    {
        using (var response = await RequestResponseMessageAsync(requestData).ConfigureAwait(false))
        {
            response.EnsureSuccessStatusCode();
        }
    }
}

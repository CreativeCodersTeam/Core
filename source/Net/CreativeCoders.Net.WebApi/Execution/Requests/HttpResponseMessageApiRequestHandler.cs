using System.Net.Http;
using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Execution.Requests;

internal class HttpResponseMessageApiRequestHandler : ApiRequestHandlerBase
{
    public HttpResponseMessageApiRequestHandler(HttpClient httpClient) : base(
        ApiMethodReturnType.HttpResponseMessage, httpClient) { }

    public override object SendRequest(RequestData requestData)
    {
        return RequestResponseMessageAsync(requestData);
    }
}

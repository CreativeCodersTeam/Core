using CreativeCoders.Net.Http;
using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Execution.Requests
{
    public class HttpResponseMessageApiRequestHandler : ApiRequestHandlerBase
    {
        public HttpResponseMessageApiRequestHandler(IHttpClient httpClient) : base(ApiMethodReturnType.HttpResponseMessage, httpClient)
        {
        }

        public override object SendRequest(RequestData requestData)
        {
            return RequestResponseMessageAsync(requestData);
        }
    }
}
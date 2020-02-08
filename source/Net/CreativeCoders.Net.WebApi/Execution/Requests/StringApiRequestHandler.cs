using System.Threading.Tasks;
using CreativeCoders.Net.Http;
using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Execution.Requests
{
    public class StringApiRequestHandler : ApiRequestHandlerBase
    {
        public StringApiRequestHandler(IHttpClient httpClient) : base(ApiMethodReturnType.String, httpClient)
        {
        }

        public override object SendRequest(RequestData requestData)
        {
            return RequestStringAsync(requestData);
        }

        private async Task<string> RequestStringAsync(RequestData requestData)
        {
            using (var response = await RequestResponseMessageAsync(requestData).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }
}
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Core.IO;
using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Execution.Requests
{
    public class StreamApiRequestHandler : ApiRequestHandlerBase
    {
        public StreamApiRequestHandler(HttpClient httpClient) : base(ApiMethodReturnType.Stream, httpClient)
        {
        }

        public override object SendRequest(RequestData requestData)
        {
            return RequestStreamAsync(requestData);
        }

        private async Task<Stream> RequestStreamAsync(RequestData requestData)
        {
            var response = await RequestResponseMessageAsync(requestData).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return new StreamWrapper(await response.Content.ReadAsStreamAsync().ConfigureAwait(false),
                null, disposing => response.Dispose());
        }
    }
}
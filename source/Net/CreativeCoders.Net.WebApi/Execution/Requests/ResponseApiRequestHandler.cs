using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Core.Reflection;
using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Execution.Requests;

public class ResponseApiRequestHandler : ApiRequestHandlerBase
{
    public ResponseApiRequestHandler(HttpClient httpClient) :
        base(ApiMethodReturnType.Response, httpClient) { }

    public override object SendRequest(RequestData requestData)
    {
        return this.ExecuteGenericMethod<object>(nameof(RequestResponseAsync),
            new[] {requestData.DataObjectType}, requestData);
    }

    private async Task<Response<T>> RequestResponseAsync<T>(RequestData requestData)
        where T : class
    {
        var response = await RequestResponseMessageAsync(requestData).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        Func<T> getData;

        if (typeof(T) == typeof(string))
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            getData = () => content as T;
        }
        else if (typeof(Stream).IsAssignableFrom(typeof(T)))
        {
            var content = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            getData = () => content as T;
        }
        else
        {
            var content = GetResponseDeserializer(requestData)
                .Deserialize<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            getData = () => content;
        }

        return new Response<T>(response, getData);
    }
}

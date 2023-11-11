using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Servers.Http;

public abstract class HttpServerBase<THttpContext> : IHttpServer
    where THttpContext : class
{
    private IHttpRequestHandler _requestHandler;

    public abstract Task StartAsync();

    public abstract Task StopAsync();

    public void RegisterRequestHandler(IHttpRequestHandler requestHandler)
    {
        _requestHandler = requestHandler;
    }

    protected async Task HandleRequestAsync(THttpContext httpContext)
    {
        var request = GetRequest(httpContext);
        var response = GetResponse(httpContext);

        await _requestHandler.ProcessAsync(request, response).ConfigureAwait(false);

        await response.Body.FlushAsync().ConfigureAwait(false);

        await FlushAndCloseOutputStreamAsync(httpContext).ConfigureAwait(false);
    }

    protected abstract Task FlushAndCloseOutputStreamAsync(THttpContext httpContext);

    protected abstract IHttpRequest GetRequest(THttpContext httpContext);

    protected abstract IHttpResponse GetResponse(THttpContext httpContext);

    public IList<string> Urls { get; } = new List<string>();
}

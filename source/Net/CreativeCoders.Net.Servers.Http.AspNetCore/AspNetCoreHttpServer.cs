using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace CreativeCoders.Net.Servers.Http.AspNetCore;

[PublicAPI]
public class AspNetCoreHttpServer : HttpServerBase<HttpContext>, IWebHostConfig, IDisposable
{
    private readonly AspNetCoreWebHost _aspNetCoreWebHost;

    public AspNetCoreHttpServer()
    {
        _aspNetCoreWebHost = new AspNetCoreWebHost(HandleRequestAsync);

        DisableLogging = true;
    }

    protected override Task FlushAndCloseOutputStreamAsync(HttpContext httpContext)
    {
        return httpContext.Response.Body.FlushAsync();
    }

    protected override IHttpRequest GetRequest(HttpContext httpContext)
    {
        return new AspNetCoreHttpRequestWrapper(httpContext.Request);
    }

    protected override IHttpResponse GetResponse(HttpContext httpContext)
    {
        return new AspNetCoreHttpResponseWrapper(httpContext.Response);
    }

    public override Task StartAsync()
    {
        return _aspNetCoreWebHost.StartAsync(this);
    }

    public override Task StopAsync()
    {
        return _aspNetCoreWebHost.StopAsync();
    }

    IReadOnlyCollection<string> IWebHostConfig.Urls => Urls.ToImmutableArray();

    public bool DisableLogging { get; set; }

    public bool AllowSynchronousIO { get; set; }

    public void Dispose()
    {
        _aspNetCoreWebHost?.Dispose();
    }
}

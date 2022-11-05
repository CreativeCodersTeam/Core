using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core.Collections;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Servers.Http.SimpleImpl;

[PublicAPI]
public sealed class SimpleHttpServer : IHttpServer, IDisposable
{
    private readonly HttpListener _httpListener;

    private IHttpRequestHandler _requestHandler;

    private Thread _listenerThread;

    private volatile bool _running;

    public SimpleHttpServer()
    {
        _httpListener = new HttpListener();

        Urls = new List<string>();
    }

    public async Task StartAsync()
    {
        _running = true;
        await Task.Run(() => _httpListener.Start()).ConfigureAwait(false);

        _listenerThread = new Thread(Listen);
        _listenerThread.Start();
    }

    private void Listen()
    {
        Urls.ForEach(url => _httpListener.Prefixes.Add(url));

        while (_running)
        {
            var context = _httpListener.GetContext();

            Task.Run(() => StartWorker(context));
        }
    }

    private async void StartWorker(HttpListenerContext context)
    {
        var request = new HttpListenerRequestWrapper(context.Request);
        var response = new HttpListenerResponseWrapper(context.Response);

        await _requestHandler.ProcessAsync(request, response).ConfigureAwait(false);

        await response.Body.FlushAsync().ConfigureAwait(false);

        await context.Response.OutputStream.FlushAsync().ConfigureAwait(false);
        context.Response.OutputStream.Close();
    }

    public async Task StopAsync()
    {
        _running = false;
        await Task.Run(() =>
        {
            _httpListener.Close();
            _httpListener.Abort();
        });
    }

    public void RegisterRequestHandler(IHttpRequestHandler requestHandler)
    {
        _requestHandler = requestHandler;
    }

    public IList<string> Urls { get; }

    public void Dispose()
    {
        (_httpListener as IDisposable)?.Dispose();
    }
}

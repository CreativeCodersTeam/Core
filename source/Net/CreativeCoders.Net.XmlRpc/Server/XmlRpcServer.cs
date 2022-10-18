using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.Net.Servers.Http;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values.Converters;
using CreativeCoders.Net.XmlRpc.Reader;
using CreativeCoders.Net.XmlRpc.Reader.Values;
using CreativeCoders.Net.XmlRpc.Writer;
using CreativeCoders.Net.XmlRpc.Writer.Values;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Server;

[PublicAPI]
public class XmlRpcServer : IXmlRpcServer, IHttpRequestHandler, IDisposable
{
    private readonly IHttpServer _httpServer;

    private readonly XmlRpcMethodExecutor _executor;

    private readonly IList<string> _allowedContentTypes;

    private readonly DataToXmlRpcValueConverter _dataToXmlRpcValueConverter;

    public XmlRpcServer(IHttpServer httpServer)
    {
        Ensure.IsNotNull(httpServer, nameof(httpServer));

        _httpServer = httpServer;
        Encoding = Encoding.UTF8;

        Urls = new List<string>();

        _httpServer.RegisterRequestHandler(this);

        Methods = new XmlRpcServerMethods();

        _executor = new XmlRpcMethodExecutor(Methods);

        _dataToXmlRpcValueConverter = new DataToXmlRpcValueConverter();

        _allowedContentTypes = new List<string>
        {
            ContentMediaTypes.Application.Xml,
            ContentMediaTypes.Text.Xml
        };
    }

    public Task StartAsync()
    {
        _httpServer.Urls.SetItems(Urls);

        return _httpServer.StartAsync();
    }

    public Task StopAsync()
    {
        return _httpServer.StopAsync();
    }

    public IList<string> Urls { get; }

    public async Task ProcessAsync(IHttpRequest request, IHttpResponse response)
    {
        Ensure.IsNotNull(request, nameof(request));
        Ensure.IsNotNull(response, nameof(response));

        if (!request.HttpMethod.Equals(HttpMethod.Post.Method, StringComparison.InvariantCultureIgnoreCase))
        {
            response.StatusCode = (int) HttpStatusCode.MethodNotAllowed;
            return;
        }

        if (!_allowedContentTypes.Any(ct =>
                request.ContentType == null ||
                request.ContentType.StartsWith(ct, StringComparison.InvariantCultureIgnoreCase)))
        {
            response.StatusCode = (int) HttpStatusCode.UnsupportedMediaType;
            return;
        }

        XmlRpcResponse xmlRpcResponse;
        try
        {
            var xmlRpcRequest = await ReadXmlRpcRequestFromBodyAsync(request).ConfigureAwait(false);

            xmlRpcResponse = await ExecuteMethods(xmlRpcRequest);
        }
        catch (Exception)
        {
            response.StatusCode = (int) HttpStatusCode.InternalServerError;

            return;
        }

        await SendResponseAsync(response, xmlRpcResponse).ConfigureAwait(false);
    }

    private async Task SendResponseAsync(IHttpResponse response, XmlRpcResponse xmlRpcResponse)
    {
        await using var contentStream = await CreateContentStreamAsync(xmlRpcResponse).ConfigureAwait(false);

        var responseStream = response.Body.GetStream();

        response.ContentType = ContentMediaTypes.Text.Xml;
        response.ContentLength = contentStream.Length;
        response.StatusCode = 200;

        await contentStream.CopyToAsync(responseStream).ConfigureAwait(false);
        await response.Body.FlushAsync().ConfigureAwait(false);
    }

    private async Task<Stream> CreateContentStreamAsync(XmlRpcResponse xmlRpcResponse)
    {
        var contentStream = new MemoryStream();
        var xmlRpcResponseWrite = new ResponseModelWriter(new ValueWriters());

        await xmlRpcResponseWrite.WriteAsync(contentStream, xmlRpcResponse, Encoding).ConfigureAwait(false);
        contentStream.Seek(0, SeekOrigin.Begin);

        return contentStream;
    }

    private async Task<XmlRpcRequest> ReadXmlRpcRequestFromBodyAsync(IHttpRequest request)
    {
        var stream = await request.Body.ReadAsStreamAsync().ConfigureAwait(false);

        var xmlRpcRequestReader = new RequestModelReader(new ValueReaders(Encoding));

        var xmlRpcRequest = await xmlRpcRequestReader.ReadAsync(stream).ConfigureAwait(false);
        return xmlRpcRequest;
    }

    private async Task<XmlRpcResponse> ExecuteMethods(XmlRpcRequest xmlRpcRequest)
    {
        var results = new List<object>();

        foreach (var methodCall in xmlRpcRequest.Methods)
        {
            var callResult = await InvokeMethod(methodCall);

            results.Add(callResult);
        }

        return new XmlRpcResponse(
            results.Select(data => new XmlRpcMethodResult(_dataToXmlRpcValueConverter.Convert(data))),
            xmlRpcRequest.IsMultiCall);
    }

    private async Task<object> InvokeMethod(XmlRpcMethodCall methodCall)
    {
        return await _executor.Invoke(methodCall);
    }

    public void Dispose()
    {
        (_httpServer as IDisposable)?.Dispose();
    }

    public IXmlRpcServerMethods Methods { get; }

    public bool SupportsListMethods { get; set; }

    public Encoding Encoding { get; set; }
}

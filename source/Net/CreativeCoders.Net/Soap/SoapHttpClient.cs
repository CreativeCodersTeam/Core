﻿using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Net.Soap.Exceptions;
using CreativeCoders.Net.Soap.Request;
using CreativeCoders.Net.Soap.Response;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Soap;

[PublicAPI]
public class SoapHttpClient : ISoapHttpClient
{
    private const string SoapActionHeader = "SOAPACTION";

    private readonly HttpClient _httpClient;

    private bool _disposed;

    public SoapHttpClient(HttpClient httpClient)
    {
        _httpClient = Ensure.NotNull(httpClient, nameof(httpClient));
    }

    public async Task<TResponse> InvokeAsync<TRequest, TResponse>(Uri uri, TRequest actionRequest)
        where TResponse : class, new()
    {
        Ensure.IsNotNull(actionRequest, nameof(actionRequest));

        var soapRequestInfo = CreateRequestInfo(actionRequest, uri);

        var response = await _httpClient
            .SendAsync(CreateRequestMessage(soapRequestInfo))
            .ConfigureAwait(false);

        var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        await using var _ = responseStream.ConfigureAwait(false);

        var responseInfo = CreateResponseInfo<TResponse>();
        var responseData = new SoapResponder<TResponse>(responseStream, responseInfo).Eval();

        return responseData;
    }

    private static HttpRequestMessage CreateRequestMessage(SoapRequestInfo soapRequestInfo)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, soapRequestInfo.Url);

        requestMessage.Headers.Add("ContentType", "text/xml; charset=utf-8");

        requestMessage.Headers.Add(SoapActionHeader,
            $"{soapRequestInfo.ServiceNameSpace}#{soapRequestInfo.ActionName}");

        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);

        new RequestXmlWriter(writer, soapRequestInfo).Write();

        stream.Seek(0, SeekOrigin.Begin);

        var content = Encoding.UTF8.GetString(stream.ToArray());

        requestMessage.Content = new StringContent(content, Encoding.UTF8, "text/xml");

        return requestMessage;
    }

    private static SoapResponseInfo CreateResponseInfo<TResponse>()
    {
        var responseType = typeof(TResponse);

        if (responseType.GetCustomAttributes(typeof(SoapResponseAttribute), false).FirstOrDefault() is
            not SoapResponseAttribute soapResponseAttribute)
        {
            throw new SoapResponseAttributeNotFoundException(responseType);
        }

        var responseInfo = new SoapResponseInfo
        {
            Name = soapResponseAttribute.Name,
            NameSpace = soapResponseAttribute.NameSpace
        };

        var propertyMappings = (from property in responseType.GetProperties()
            let attribute = property.GetCustomAttribute<SoapResponseFieldAttribute>(false)
            where attribute != null
            select new PropertyFieldMapping {FieldName = attribute.Name, Property = property}).ToArray();

        responseInfo.PropertyMappings = propertyMappings;

        return responseInfo;
    }

    private static SoapRequestInfo CreateRequestInfo<TRequest>(TRequest actionRequest, Uri uri)
    {
        var requestType = typeof(TRequest);

        if (requestType.GetCustomAttributes(typeof(SoapRequestAttribute), false)
                .FirstOrDefault() is not SoapRequestAttribute soapRequestAttribute)
        {
            throw new SoapRequestAttributeNotFoundException(requestType);
        }

        var requestInfo = new SoapRequestInfo
        {
            Url = uri,
            Action = actionRequest,
            ActionName = soapRequestAttribute.Name,
            ServiceNameSpace = soapRequestAttribute.NameSpace
        };

        var propertyMappings = (from property in requestType.GetProperties()
            let attribute = property.GetCustomAttribute<SoapRequestFieldAttribute>(false)
            where attribute != null
            select new PropertyFieldMapping {FieldName = attribute.Name, Property = property}).ToArray();

        requestInfo.PropertyMappings = propertyMappings;

        return requestInfo;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || _disposed)
        {
            return;
        }

        _disposed = true;
        _httpClient.Dispose();
    }
}

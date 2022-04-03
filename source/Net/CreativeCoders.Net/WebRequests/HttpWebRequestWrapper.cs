//using System;
//using System.IO;
//using System.Net;
//using System.Net.Security;

//namespace CreativeCoders.Net.WebRequests;

//public class HttpWebRequestWrapper : IHttpWebRequest
//{
//    private readonly HttpWebRequest _httpWebRequest;

//    public HttpWebRequestWrapper(HttpWebRequest httpWebRequest)
//    {
//        _httpWebRequest = httpWebRequest;
//    }

//    public ICredentials Credentials
//    {
//        get => _httpWebRequest.Credentials;
//        set => _httpWebRequest.Credentials = value;
//    }

//    public string Method
//    {
//        get => _httpWebRequest.Method;
//        set => _httpWebRequest.Method = value;
//    }

//    public string ContentType
//    {
//        get => _httpWebRequest.ContentType;
//        set => _httpWebRequest.ContentType = value;
//    }

//    public WebHeaderCollection Headers
//    {
//        get => _httpWebRequest.Headers;
//        set => _httpWebRequest.Headers = value;
//    }

//    public Version ProtocolVersion
//    {
//        get => _httpWebRequest.ProtocolVersion;
//        set => _httpWebRequest.ProtocolVersion = value;
//    }

//    public RemoteCertificateValidationCallback ServerCertificateValidationCallback
//    {
//        get => _httpWebRequest.ServerCertificateValidationCallback;
//        set => _httpWebRequest.ServerCertificateValidationCallback = value;
//    }

//    public Stream GetRequestStream()
//    {
//        return _httpWebRequest.GetRequestStream();
//    }

//    public WebResponse GetResponse()
//    {
//        return _httpWebRequest.GetResponse();
//    }

//    public IWebProxy Proxy
//    {
//        get => _httpWebRequest.Proxy;
//        set => _httpWebRequest.Proxy = value;
//    }

//    public int Timeout
//    {
//        get => _httpWebRequest.Timeout;
//        set => _httpWebRequest.Timeout = value;
//    }
//}

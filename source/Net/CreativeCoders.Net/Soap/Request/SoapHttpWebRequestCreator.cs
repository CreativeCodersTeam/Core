using System.Net;
using CreativeCoders.Core;
using CreativeCoders.Net.WebRequests;

namespace CreativeCoders.Net.Soap.Request;

internal static class SoapHttpWebRequestCreator
{
    public static IHttpWebRequest Create(SoapRequestInfo soapRequestInfo, IWebRequestFactory webRequestFactory)
    {
        Ensure.IsNotNull(soapRequestInfo, nameof(soapRequestInfo));

        var httpWebRequest = webRequestFactory.CreateHttpWebRequest(soapRequestInfo.Url);
        httpWebRequest.Credentials = soapRequestInfo.Credentials;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "text/xml; charset=utf-8";
        httpWebRequest.Headers.Add("SOAPACTION", $"{soapRequestInfo.ServiceNameSpace}#{soapRequestInfo.ActionName}");
        httpWebRequest.ProtocolVersion = HttpVersion.Version11;

        return httpWebRequest;
    }
}
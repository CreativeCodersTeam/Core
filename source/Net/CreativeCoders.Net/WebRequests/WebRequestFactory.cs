using System.Net;

namespace CreativeCoders.Net.WebRequests;

public class WebRequestFactory : IWebRequestFactory
{
    public static IWebRequestFactory Default { get; } = new WebRequestFactory();

    public IHttpWebRequest CreateHttpWebRequest(string url)
    {
        var httpWebRequest = WebRequest.CreateHttp(url);
        return new HttpWebRequestWrapper(httpWebRequest);
    }
}

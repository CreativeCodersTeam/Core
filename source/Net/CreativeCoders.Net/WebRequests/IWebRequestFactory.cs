namespace CreativeCoders.Net.WebRequests;

public interface IWebRequestFactory
{
    IHttpWebRequest CreateHttpWebRequest(string url);
}
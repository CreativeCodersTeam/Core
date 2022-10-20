using System.Net.Http;
using CreativeCoders.Net.WebApi.Serialization;

namespace CreativeCoders.Net.WebApi.Execution;

public class ApiData
{
    public string BaseUri { get; init; }

    public HttpClient HttpClient { get; init; }

    public IDataFormatter DefaultDataFormatter { get; init; }
}

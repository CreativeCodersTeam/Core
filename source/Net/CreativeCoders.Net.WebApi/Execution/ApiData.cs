using CreativeCoders.Net.Http;
using CreativeCoders.Net.WebApi.Serialization;

namespace CreativeCoders.Net.WebApi.Execution
{
    public class ApiData
    {
        public string BaseUri { get; set; }

        public IHttpClient HttpClient { get; set; }

        public IDataFormatter DefaultDataFormatter { get; set; }
    }
}
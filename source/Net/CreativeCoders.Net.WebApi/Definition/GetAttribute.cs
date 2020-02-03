using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Definition
{
    [PublicAPI]
    public class GetAttribute : ApiMethodBaseAttribute
    {
        public GetAttribute(string uri) : base(HttpRequestMethod.Get, uri)
        {
        }
    }
}
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Definition
{
    [PublicAPI]
    public class PutAttribute : ApiMethodBaseAttribute
    {
        public PutAttribute(string uri) : base(HttpRequestMethod.Put, uri)
        {
        }
    }
}
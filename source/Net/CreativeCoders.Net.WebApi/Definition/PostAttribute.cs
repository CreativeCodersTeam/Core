using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Definition
{
    [PublicAPI]
    public class PostAttribute : ApiMethodBaseAttribute
    {
        public PostAttribute(string uri) : base(HttpRequestMethod.Post, uri)
        {
        }
    }
}
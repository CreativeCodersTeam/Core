using System.Net.Http;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace CreativeCoders.Net.Http
{
    [PublicAPI]
    public class JsonContent : StringContent
    {
        public JsonContent(object data) : this(data, Encoding.UTF8)
        {
        }

        public JsonContent(object data, Encoding encoding) : base(JsonConvert.SerializeObject(data), encoding, ContentMediaTypes.Application.Json)
        {
        }
    }
}
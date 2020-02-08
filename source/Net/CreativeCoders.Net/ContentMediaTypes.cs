using System;
using System.Linq;

namespace CreativeCoders.Net
{
    public static class ContentMediaTypes
    {
        public static class Application
        {
            public const string Xml = "application/xml";

            public const string Json = "application/json";

            public const string OctetStream = "application/octet-stream";
        }

        public static class Text
        {
            public const string Xml = "text/xml";

            public const string Plain = "text/plain";
        }

        public static bool IsContentType(string contentTypeToCheck, string contentType)
        {
            var contentTypeParts = contentTypeToCheck
                .Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim());

            return contentTypeParts.Any(x => x.Equals(contentType, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
using System;
using System.Net.Http;

namespace CreativeCoders.Net.WebApi.Definition
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ApiMethodBaseAttribute : Attribute
    {
        private HttpCompletionOption _completionOption;

        private bool _completionOptionIsSet;

        protected ApiMethodBaseAttribute(HttpRequestMethod requestMethod, string uri)
        {
            RequestMethod = requestMethod;
            Uri = uri;
        }

        public HttpRequestMethod RequestMethod { get; }

        public string Uri { get; }

        public Type ResponseDataFormatterType { get; set; }

        public string ResponseDataFormat { get; set; }

        public HttpCompletionOption CompletionOption
        {
            get => _completionOption;
            set
            {
                _completionOption = value;
                _completionOptionIsSet = true;
            }
        }

        public HttpCompletionOption GetCompletionOption(HttpCompletionOption defaultCompletionOption)
        {
            return _completionOptionIsSet
                ? _completionOption
                : defaultCompletionOption;
        }
    }
}
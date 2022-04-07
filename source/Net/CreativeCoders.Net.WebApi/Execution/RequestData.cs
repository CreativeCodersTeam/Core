using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using CreativeCoders.Net.WebApi.Definition;
using CreativeCoders.Net.WebApi.Serialization;
using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Execution;

public class RequestData
{
    public IEnumerable<RequestHeader> Headers { get; set; }

    public Uri RequestUri { get; set; }

    public HttpRequestMethod RequestMethod { get; set; }

    public ApiMethodReturnType RequestReturnType { get; set; }

    public Type DataObjectType { get; set; }

    public IDataDeserializer ResponseDeserializer { get; set; }

    public IDataFormatter DefaultDataFormatter { get; set; }

    public Func<object> GetBodyValue { get; set; }

    public CancellationToken CancellationToken { get; set; }

    public HttpCompletionOption CompletionOption { get; set; }
}

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
    public IEnumerable<RequestHeader> Headers { get; init; }

    public Uri RequestUri { get; init; }

    public HttpRequestMethod RequestMethod { get; init; }

    public ApiMethodReturnType RequestReturnType { get; init; }

    public Type DataObjectType { get; init; }

    public IDataDeserializer ResponseDeserializer { get; set; }

    public IDataFormatter DefaultDataFormatter { get; init; }

    public Func<object> GetBodyValue { get; init; }

    public CancellationToken CancellationToken { get; init; }

    public HttpCompletionOption CompletionOption { get; init; }
}

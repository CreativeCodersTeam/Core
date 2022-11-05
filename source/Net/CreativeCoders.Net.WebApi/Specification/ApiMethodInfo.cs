using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using CreativeCoders.Net.WebApi.Definition;
using CreativeCoders.Net.WebApi.Specification.Parameters;

namespace CreativeCoders.Net.WebApi.Specification;

internal class ApiMethodInfo
{
    public MethodInfo Method { get; init; }

    public ApiMethodReturnType ReturnType { get; init; }

    public HttpRequestMethod RequestMethod { get; init; }

    public IEnumerable<RequestHeader> HeaderDefinitions { get; init; }

    public ApiMethodArgumentInfo[] ArgumentInfos { get; set; }

    public string MethodUri { get; init; }

    public ParameterBodyDefinition Body { get; set; }

    public ParameterCancellationTokenDefinition CancellationToken { get; set; }

    public ParameterCompletionOptionDefinition CompletionOption { get; set; }

    public HttpCompletionOption DefaultCompletionOption { get; set; }
}

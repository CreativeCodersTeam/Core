using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using CreativeCoders.Net.WebApi.Definition;
using CreativeCoders.Net.WebApi.Specification.Parameters;

namespace CreativeCoders.Net.WebApi.Specification;

public class ApiMethodInfo
{
    public MethodInfo Method { get; set; }

    public ApiMethodReturnType ReturnType { get; set; }

    public HttpRequestMethod RequestMethod { get; set; }

    public IEnumerable<RequestHeader> HeaderDefinitions { get; set; }

    public ApiMethodArgumentInfo[] ArgumentInfos { get; set; }

    public string MethodUri { get; set; }

    public ParameterBodyDefinition Body { get; set; }

    public ParameterCancellationTokenDefinition CancellationToken { get; set; }

    public ParameterCompletionOptionDefinition CompletionOption { get; set; }

    public HttpCompletionOption DefaultCompletionOption { get; set; }
}

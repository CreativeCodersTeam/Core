using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using CreativeCoders.Core.SysEnvironment;
using JetBrains.Annotations;

namespace CreativeCoders.Net.JsonRpc;

[ExcludeFromCodeCoverage]
[PublicAPI]
public class JsonRpcRequest
{
    public JsonRpcRequest(string method, object?[] arguments)
    {
        Id = Env.TickCount;
        Method = method;
        Arguments = arguments;
    }

    public int Id { get; set; }

    public string Method { get; set; }

    [JsonPropertyName("params")]
    public object?[] Arguments { get; set; }
}

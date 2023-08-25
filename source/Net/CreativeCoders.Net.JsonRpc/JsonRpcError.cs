using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.Net.JsonRpc;

[ExcludeFromCodeCoverage]
[PublicAPI]
public class JsonRpcError
{
    public string? Name { get; set; }

    public int Code { get; set; }

    public string? Message { get; set; }
}

using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Net.JsonRpc;

[ExcludeFromCodeCoverage]
public class JsonRpcError
{
    public string? Name { get; set; }

    public int Code { get; set; }

    public string? Message { get; set; }
}

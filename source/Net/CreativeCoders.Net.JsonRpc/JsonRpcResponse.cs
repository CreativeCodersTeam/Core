using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.Net.JsonRpc;

[PublicAPI]
[ExcludeFromCodeCoverage]
public class JsonRpcResponse<T>
{
    public int Id { get; set; }

    public T? Result { get; set; }

    public JsonRpcError? Error { get; set; }
}

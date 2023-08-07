using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.JsonRpc.ApiBuilder;

[PublicAPI]
public interface IJsonRpcApiBuilder<out T>
    where T : class
{
    IJsonRpcApiBuilder<T> ForUrl(Uri url);

    T Build();
}

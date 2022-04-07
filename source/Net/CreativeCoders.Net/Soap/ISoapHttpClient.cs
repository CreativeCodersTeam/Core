using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Soap;

[PublicAPI]
public interface ISoapHttpClient : IDisposable
{
    Task<TResponse> InvokeAsync<TRequest, TResponse>(Uri uri, TRequest actionRequest)
        where TResponse : class, new();
}

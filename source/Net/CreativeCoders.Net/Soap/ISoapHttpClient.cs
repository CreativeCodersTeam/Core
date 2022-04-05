using System;
using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Soap;

[PublicAPI]
public interface ISoapHttpClient : IDisposable
{
    //ICredentials Credentials { get; set; }

    string Url { get; set; }

    //bool AllowUntrustedCertificates { get; set; }

    Task<TResponse> InvokeAsync<TRequest, TResponse>(TRequest actionRequest)
        where TResponse : class, new();
}

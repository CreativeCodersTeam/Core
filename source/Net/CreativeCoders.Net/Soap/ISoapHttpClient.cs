using System.Net;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Soap;

[PublicAPI]
public interface ISoapHttpClient
{
    ICredentials Credentials { get; set; }

    string Url { get; set; }

    bool AllowUntrustedCertificates { get; set; }

    TResponse Invoke<TRequest, TResponse>(TRequest actionRequest) where TResponse : class, new();
}
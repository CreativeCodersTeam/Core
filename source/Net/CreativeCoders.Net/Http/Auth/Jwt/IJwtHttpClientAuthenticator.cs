using System;
using System.Net;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http.Auth.Jwt;

[PublicAPI]
public interface IJwtHttpClientAuthenticator : IHttpClientAuthenticator
{
    Uri TokenRequestUri { get; set; }

    ICredentials Credentials { get; set; }
}

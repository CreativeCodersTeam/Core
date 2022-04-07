using System;
using System.Net;

namespace CreativeCoders.Net.Http.Auth.Jwt;

public interface IJwtHttpClientAuthenticator : IHttpClientAuthenticator
{
    Uri TokenRequestUri { get; set; }

    ICredentials Credentials { get; set; }
}

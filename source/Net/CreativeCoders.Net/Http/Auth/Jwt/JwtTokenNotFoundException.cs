using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http.Auth.Jwt;

[PublicAPI]
public class JwtTokenNotFoundException : Exception
{
    internal JwtTokenNotFoundException(string tokenPropertyName, string response)
        : base($"Jwt token not found in property '{tokenPropertyName}'")
    {
        TokenPropertyName = tokenPropertyName;
        Response = response;
    }

    public string Response { get; }

    public string TokenPropertyName { get; set; }
}

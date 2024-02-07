using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Api;

[PublicAPI]
[ExcludeFromCodeCoverage]
public class LoginRequest
{
    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Domain { get; set; }

    public bool UseCookies { get; set; }
}

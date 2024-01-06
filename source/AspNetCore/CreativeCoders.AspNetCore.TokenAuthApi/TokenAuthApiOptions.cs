using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.TokenAuthApi;

[PublicAPI]
[ExcludeFromCodeCoverage]
public class TokenAuthApiOptions
{
    public bool UseCookies { get; set; }

    public string? CookieDomain { get; set; }

    public string CookiePath { get; set; } = "/";

    public bool UseRefreshTokens { get; set; }

    public string AuthTokenName { get; set; } = "auth_token";

    public string RefreshTokenName { get; set; } = "refresh_auth_token";

    public string Issuer { get; set; } = string.Empty;
}

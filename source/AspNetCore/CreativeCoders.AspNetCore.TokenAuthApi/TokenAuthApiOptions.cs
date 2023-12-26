using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.TokenAuthApi;

[PublicAPI]
public class TokenAuthApiOptions
{
    public bool UseCookies { get; set; }

    public bool UseRefreshTokens { get; set; }

    public string AuthTokenName { get; set; } = "auth_token";

    public string RefreshTokenName { get; set; } = "refresh_auth_token";
}

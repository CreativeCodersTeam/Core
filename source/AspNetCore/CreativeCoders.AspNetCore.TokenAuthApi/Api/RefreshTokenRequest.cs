using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Api;

[PublicAPI]
public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

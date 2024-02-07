using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.TokenAuth.Jwt;

public class JwtAuthenticationOptions
{
    public SecurityKey? SecurityKey { get; set; }

    public bool UseCookies { get; set; }

    public string AuthTokenName { get; set; } = "jwt_auth_token";
}

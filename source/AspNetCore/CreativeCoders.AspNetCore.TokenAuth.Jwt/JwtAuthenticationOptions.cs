using JetBrains.Annotations;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.TokenAuth.Jwt;

[PublicAPI]
public class JwtAuthenticationOptions
{
    public SecurityKey? SecurityKey { get; set; }

    public string AuthTokenName { get; set; } = "cc_auth_token";

    public string Issuer { get; set; } = "cc-token-auth-api";
}

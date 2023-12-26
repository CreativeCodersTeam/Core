using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.TokenAuth.Jwt;

public class JwtAuthenticationOptions
{
    public SecurityKey? SecurityKey { get; set; }
}

using JetBrains.Annotations;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Jwt;

[PublicAPI]
public class JwtTokenAuthOptions
{
    public SecurityKey? SecurityKey { get; set; }
}

using JetBrains.Annotations;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Jwt;

[PublicAPI]
public class JwtTokenAuthApiOptions
{
    public SecurityKey? SecurityKey { get; set; }
}

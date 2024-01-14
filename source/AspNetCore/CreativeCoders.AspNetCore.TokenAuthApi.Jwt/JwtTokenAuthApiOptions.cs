using JetBrains.Annotations;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Jwt;

/// <summary>
/// Options for configuring JWT token authorization API.
/// </summary>
[PublicAPI]
public class JwtTokenAuthApiOptions
{
    /// <summary>
    /// Gets or sets the security key for token creation and validation.
    /// </summary>
    public SecurityKey? SecurityKey { get; set; }
}

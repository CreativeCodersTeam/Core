using System;
using CreativeCoders.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Jwt;

public class JwtBearerOptionsConfiguration : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly SecurityKey? _securityKey;

    public JwtBearerOptionsConfiguration(IOptions<JwtTokenAuthOptions> options)
    {
        _securityKey = Ensure.NotNull(options).Value.SecurityKey;

        if (_securityKey == null)
        {
            throw new InvalidOperationException("Security key must not be null");
        }
    }

    public void Configure(JwtBearerOptions options)
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _securityKey,
            ValidIssuer = string.Empty,
            ValidAudience = string.Empty,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        Configure(options);
    }
}

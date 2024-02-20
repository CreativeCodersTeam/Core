using CreativeCoders.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.TokenAuth.Jwt;

[UsedImplicitly]
public class JwtBearerOptionsConfiguration : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtAuthenticationOptions _options;

    public JwtBearerOptionsConfiguration(IOptions<JwtAuthenticationOptions> options)
    {
        _options = Ensure.NotNull(options).Value;

        if (_options.SecurityKey == null)
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
            IssuerSigningKey = _options.SecurityKey,
            ValidIssuer = _options.Issuer,
            ValidAudience = string.Empty,
            ValidateIssuer = true,
            ValidateAudience = false
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.HttpContext.Request.GetJwtToken(_options.AuthTokenName);

                return Task.CompletedTask;
            }
        };
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        Configure(options);
    }
}

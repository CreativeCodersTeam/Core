using CreativeCoders.Core;
using CreativeCoders.Core.Tasking;
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
            ValidIssuer = string.Empty,
            ValidAudience = string.Empty,
            ValidateIssuer = false,
            ValidateAudience = false
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.HttpContext.Request.Headers.Authorization.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(token))
                {
                    token = context.HttpContext.Request.Cookies[_options.AuthTokenName];
                }

                context.Token = token;

                return Task.CompletedTask;
            }
        };
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        Configure(options);
    }
}

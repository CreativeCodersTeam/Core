using System.Text;
using CreativeCoders.AspNetCore.TokenAuth.Abstractions;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.TokenAuth.Jwt;

[PublicAPI]
public static class JwtServicesCollectionExtensions
{
    public static IServiceCollection AddJwtTokenAuthentication(
        this IServiceCollection services, string symSecurityKey)
    {
        services
            .AddSingleton<ISymSecurityKeyConfig>(_ => new SymSecurityKeyConfig(symSecurityKey))
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //.AddJwtBearer();
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(symSecurityKey)),
                    ValidIssuer = string.Empty,
                    ValidAudience = string.Empty,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                // x.Events = new JwtBearerEvents
                // {
                //     OnMessageReceived = context =>
                //     {
                //         context.Token = context.Request.Cookies["jwt"];
                //         return Task.CompletedTask;
                //     }
                // };
            });

        services.TryAddTransient<IConfigureOptions<JwtBearerOptions>, JwtBearerConfiguration>();

        return services;
    }
}

public class JwtBearerConfiguration : IConfigureOptions<JwtBearerOptions>
{
    private readonly ISymSecurityKeyConfig _symSecurityKeyConfig;

    public JwtBearerConfiguration(ISymSecurityKeyConfig symSecurityKeyConfig)
    {
        _symSecurityKeyConfig = Ensure.NotNull(symSecurityKeyConfig);
    }

    public void Configure(JwtBearerOptions options)
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_symSecurityKeyConfig.SecurityKey)),
            ValidIssuer = string.Empty,
            ValidAudience = string.Empty,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
}

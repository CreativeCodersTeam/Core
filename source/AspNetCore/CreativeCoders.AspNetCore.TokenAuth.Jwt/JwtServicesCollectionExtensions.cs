using System.Text;
using CreativeCoders.AspNetCore.TokenAuth.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.TokenAuth.Jwt;

[PublicAPI]
public static class JwtServicesCollectionExtensions
{
    public static IServiceCollection AddJwtSupport<TUserAuthProvider, TUserClaimsProvider>(
        this IServiceCollection services, string symSecurityKey)
        where TUserAuthProvider : class, IUserAuthProvider
        where TUserClaimsProvider: class, IUserClaimsProvider
    {
        services
            .AddScoped<IUserAuthProvider, TUserAuthProvider>()
            .AddScoped<IUserClaimsProvider, TUserClaimsProvider>()
            .AddSingleton<ISymSecurityKeyConfig>(_ => new SymSecurityKeyConfig(symSecurityKey))
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
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

        return services;
    }

    public static IMvcBuilder AddJwtSupport<TUserAuthProvider, TUserClaimsProvider>(this IMvcBuilder builder,
        string symSecurityKey)
        where TUserAuthProvider : class, IUserAuthProvider
        where TUserClaimsProvider : class, IUserClaimsProvider
    {
        builder.Services
            .AddJwtSupport<TUserAuthProvider, TUserClaimsProvider>(symSecurityKey);

        return builder;
    }
}

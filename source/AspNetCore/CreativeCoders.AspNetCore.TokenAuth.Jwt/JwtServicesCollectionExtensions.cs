using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.AspNetCore.TokenAuth.Jwt;

[PublicAPI]
public static class JwtServicesCollectionExtensions
{
    public static IServiceCollection AddJwtTokenAuthentication(
        this IServiceCollection services, Action<JwtAuthenticationOptions>? configureOptions = null)
    {
        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        if (configureOptions is not null)
        {
            services.Configure(configureOptions);
        }

        services.ConfigureOptions<JwtBearerOptionsConfiguration>();

        return services;
    }
}

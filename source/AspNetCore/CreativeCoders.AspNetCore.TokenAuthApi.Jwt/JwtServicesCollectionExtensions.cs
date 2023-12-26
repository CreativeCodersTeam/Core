using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Jwt;

[PublicAPI]
public static class JwtServicesCollectionExtensions
{
    public static IServiceCollection AddJwtTokenAuthentication(
        this IServiceCollection services)
    {
        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

            services.ConfigureOptions<JwtBearerOptionsConfiguration>();

        return services;
    }
}

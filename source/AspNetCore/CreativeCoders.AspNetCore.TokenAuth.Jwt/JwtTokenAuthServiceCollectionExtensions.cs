using System;
using CreativeCoders.AspNetCore.TokenAuth.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace CreativeCoders.AspNetCore.TokenAuth.Jwt;

[PublicAPI]
public static class JwtTokenAuthServiceCollectionExtensions
{
    public static void AddJwtTokenAuthApi(this IServiceCollection services, Action<TokenAuthOptions> configureOptions)
    {
        services.TryAddScoped<ITokenCreator, JwtTokenCreator>();
        services.AddTokenAuthApi(configureOptions);
    }

    public static void AddJwtTokenAuthApi<TConfig>(this IServiceCollection services)
        where TConfig : class, IConfigureOptions<TokenAuthOptions>
    {
        services.TryAddScoped<ITokenCreator, JwtTokenCreator>();
        services.AddTokenAuthApi<TConfig>();
    }
}

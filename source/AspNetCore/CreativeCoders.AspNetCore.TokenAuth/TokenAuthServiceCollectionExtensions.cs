using System;
using CreativeCoders.AspNetCore.TokenAuth.Abstractions;
using CreativeCoders.AspNetCore.TokenAuth.Api;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace CreativeCoders.AspNetCore.TokenAuth;

[PublicAPI]
public static class TokenAuthServiceCollectionExtensions
{
    public static void AddTokenAuthApi(this IServiceCollection services, Action<TokenAuthOptions> configureOptions)
    {
        Ensure.NotNull(services);

        services.AddOptions();

        services.TryAddScoped<ITokenAuthHandler, DefaultTokenAuthHandler>();

        services.Configure(configureOptions);
    }

    public static void AddTokenAuthApi<TConfig>(this IServiceCollection services)
        where TConfig : class, IConfigureOptions<TokenAuthOptions>
    {
        Ensure.NotNull(services);

        services.AddOptions();

        services.TryAddScoped<ITokenAuthHandler, DefaultTokenAuthHandler>();

        services.TryAddTransient<IConfigureOptions<TokenAuthOptions>, TConfig>();
    }

    public static IMvcBuilder AddTokenAuthApiController(this IMvcBuilder mvcBuilder)
    {
        Ensure.NotNull(mvcBuilder);

        return mvcBuilder.AddApplicationPart(typeof(TokenAuthController).Assembly);
    }
}

using System;
using CreativeCoders.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.AspNetCore.TokenAuth;

public static class TokenAuthServiceCollectionExtensions
{
    public static void AddTokenAuth(this IServiceCollection services, Action<TokenAuthOptions> configureOptions)
    {
        Ensure.NotNull(services);

        services.TryAddScoped<ITokenAuthHandler, DefaultTokenAuthHandler>();
        services.Configure(configureOptions);
    }

    public static IMvcBuilder AddTokenAuthController(this IMvcBuilder mvcBuilder)
    {
        Ensure.NotNull(mvcBuilder);

        return mvcBuilder.AddApplicationPart(typeof(TokenAuthController).Assembly);
    }
}

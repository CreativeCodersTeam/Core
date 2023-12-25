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
public static class TokenAuthApiExtensions
{
    public static TokenAuthApiBuilder AddTokenAuthApi(this IServiceCollection services)
    {
        Ensure.NotNull(services);

        services.AddOptions();

        services.TryAddScoped<ITokenAuthHandler, DefaultTokenAuthHandler>();

        return new TokenAuthApiBuilder(services);
    }

    public static TokenAuthApiBuilder ConfigureOptions(this TokenAuthApiBuilder tokenAuthApiBuilder,
        Action<TokenAuthOptions> configureOptions)
    {
        tokenAuthApiBuilder.Services.Configure(configureOptions);

        return tokenAuthApiBuilder;
    }

    public static TokenAuthApiBuilder ConfigureOptions<TConfig>(this TokenAuthApiBuilder tokenAuthApiBuilder)
        where TConfig : class, IConfigureOptions<TokenAuthOptions>
    {
        tokenAuthApiBuilder.Services.TryAddTransient<IConfigureOptions<TokenAuthOptions>, TConfig>();

        return tokenAuthApiBuilder;
    }

    public static IMvcBuilder AddTokenAuthApiController(this IMvcBuilder mvcBuilder)
    {
        Ensure.NotNull(mvcBuilder);

        return mvcBuilder.AddApplicationPart(typeof(TokenAuthController).Assembly);
    }
}

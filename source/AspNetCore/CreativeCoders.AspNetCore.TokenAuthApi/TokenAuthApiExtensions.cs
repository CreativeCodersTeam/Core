using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.AspNetCore.TokenAuthApi.Api;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.AspNetCore.TokenAuthApi;

[PublicAPI]
public static class TokenAuthApiExtensions
{
    /// <summary>
    /// Adds the a token based auth Api to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <returns>A TokenAuthApiBuilder instance that can be used to configure the TokenAuthApi.</returns>
    public static TokenAuthApiBuilder AddTokenAuthApi(this IServiceCollection services)
    {
        Ensure.NotNull(services);

        services.AddOptions();

        services.TryAddScoped<ITokenAuthHandler, DefaultTokenAuthHandler>();

        return new TokenAuthApiBuilder(services);
    }

    public static TokenAuthApiBuilder ConfigureOptions(this TokenAuthApiBuilder tokenAuthApiBuilder,
        Action<TokenAuthApiOptions> configureOptions)
    {
        tokenAuthApiBuilder.Services.Configure(configureOptions);

        return tokenAuthApiBuilder;
    }

    /// <summary>
    /// Extension method to add the TokenAuthApiController to the IMvcBuilder.
    /// </summary>
    /// <param name="mvcBuilder">The IMvcBuilder instance.</param>
    /// <returns>The IMvcBuilder instance with the TokenAuthApiController added as an application part.</returns>
    public static IMvcBuilder AddTokenAuthApiController(this IMvcBuilder mvcBuilder)
    {
        Ensure.NotNull(mvcBuilder);

        return mvcBuilder.AddApplicationPart(typeof(TokenAuthController).Assembly);
    }
}

using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.AspNetCore.TokenAuthApi.Api;
using CreativeCoders.AspNetCore.TokenAuthApi.Default;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.AspNetCore.TokenAuthApi;

[PublicAPI]
public static class TokenAuthApiExtensions
{
    /// <summary>
    ///     Adds token-based authentication API capabilities to the provided <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the token-based authentication API services to.</param>
    /// <param name="configureOptions">
    ///     An optional <see cref="Action{T}" /> delegate that is used to configure the
    ///     <see cref="TokenAuthApiOptions" />.
    /// </param>
    /// <exception cref="System.ArgumentNullException"><paramref name="services" /> argument is null.</exception>
    /// <remarks>
    ///     This method adds the necessary services to the IServiceCollection to enable token-based authentication API.
    ///     Services added include options configuration, scoped default token authentication handler, and any supplied
    ///     configurable options.
    /// </remarks>
    public static void AddTokenAuthApi(this IServiceCollection services,
        Action<TokenAuthApiOptions>? configureOptions = null)
    {
        Ensure.NotNull(services);

        services.AddOptions();

        services.TryAddScoped<ITokenAuthHandler, DefaultTokenAuthHandler>();

        services.TryAddScoped<IUserProvider, DefaultUserProvider>();

        services.TryAddSingleton<IRefreshTokenStore, InMemoryRefreshTokenStore>();

        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }
    }

    /// <summary>
    ///     Extension method to add the TokenAuthApiController to the IMvcBuilder.
    /// </summary>
    /// <param name="mvcBuilder">The IMvcBuilder instance.</param>
    /// <returns>The IMvcBuilder instance with the TokenAuthApiController added as an application part.</returns>
    public static IMvcBuilder AddTokenAuthApiController(this IMvcBuilder mvcBuilder)
    {
        Ensure.NotNull(mvcBuilder);

        return mvcBuilder.AddApplicationPart(typeof(TokenAuthController).Assembly);
    }
}

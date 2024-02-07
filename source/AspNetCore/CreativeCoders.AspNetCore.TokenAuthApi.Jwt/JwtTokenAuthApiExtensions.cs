using System;
using System.Diagnostics.CodeAnalysis;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Jwt;

[ExcludeFromCodeCoverage]
public static class JwtTokenAuthApiExtensions
{
    /// <summary>
    ///     Adds a JWT token authentication as well as service configurations to the provided services collection.
    /// </summary>
    /// <param name="services">
    ///     The service collection where the token auth api and the JWT token creator are registered. An
    ///     <see cref="IServiceCollection" />.
    /// </param>
    /// <param name="configureJwtApiOptions">
    ///     The <see cref="JwtTokenAuthApiOptions" /> configuration action that sets different
    ///     options related to JWT token generation.
    /// </param>
    /// <param name="configureTokenApiOptions">The <see cref="TokenAuthApiOptions" /> to configure the token authentication.</param>
    /// <exception cref="ArgumentException"><paramref name="services" /> parameter is null.</exception>
    /// <remarks>
    ///     This is an extension method for <see cref="IServiceCollection" /> which sets up JWT token authentication.
    ///     The method registers a <see cref="JwtTokenCreator" /> scoped service and invokes the token API's configuration
    ///     methods.
    ///     This configuration includes settings for both general token API settings and specific JWT token settings.
    ///     Please note that if either configuration action is not provided, the method uses default values.
    ///     Some configurations must be necessarily be provided.
    /// </remarks>
    public static void AddJwtTokenAuthApi(
        this IServiceCollection services, Action<JwtTokenAuthApiOptions>? configureJwtApiOptions = null,
        Action<TokenAuthApiOptions>? configureTokenApiOptions = null)
    {
        services.TryAddScoped<ITokenCreator, JwtTokenCreator>();

        if (configureJwtApiOptions != null)
        {
            services.Configure(configureJwtApiOptions);
        }

        services.AddTokenAuthApi(configureTokenApiOptions);
    }
}

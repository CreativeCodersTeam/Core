using System.Diagnostics.CodeAnalysis;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Jwt;

[ExcludeFromCodeCoverage]
public static class JwtTokenAuthApiExtensions
{
    /// <summary>
    /// Adds a JWT token authentication API.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>A builder to configure the token authentication.</returns>
    public static TokenAuthApiBuilder AddJwtTokenAuthApi(
        this IServiceCollection services)
    {
        services.TryAddScoped<ITokenCreator, JwtTokenCreator>();

        return services.AddTokenAuthApi();
    }
}

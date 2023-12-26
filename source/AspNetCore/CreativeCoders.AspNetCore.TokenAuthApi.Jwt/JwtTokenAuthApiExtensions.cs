using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Jwt;

public static class JwtTokenAuthApiExtensions
{
    public static TokenAuthApiBuilder AddJwtTokenAuthApi<TUserAuthProvider, TUserClaimsProvider>(
        this IServiceCollection services)
        where TUserAuthProvider : class, IUserAuthProvider
        where TUserClaimsProvider: class, IUserClaimsProvider
    {
        services.TryAddScoped<IUserAuthProvider, TUserAuthProvider>();
        services.TryAddScoped<IUserClaimsProvider, TUserClaimsProvider>();

        services.TryAddScoped<ITokenCreator, JwtTokenCreator>();

        return services.AddTokenAuthApi();
    }
}

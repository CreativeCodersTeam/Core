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

    public static IMvcBuilder AddTokenAuthApiController(this IMvcBuilder mvcBuilder)
    {
        Ensure.NotNull(mvcBuilder);

        return mvcBuilder.AddApplicationPart(typeof(TokenAuthController).Assembly);
    }
}

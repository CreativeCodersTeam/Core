using CreativeCoders.Core;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.AspNetCore.TokenAuthApi;

public class TokenAuthApiBuilder
{
    internal TokenAuthApiBuilder(IServiceCollection services)
    {
        Services = Ensure.NotNull(services);
    }

    public IServiceCollection Services { get; }
}

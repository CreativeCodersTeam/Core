using CreativeCoders.Cli.Hosting.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Cli.Hosting;

public static class CliHostingServiceCollectionExtensions
{
    public static IServiceCollection AddCliHosting(this IServiceCollection services)
    {
        services.TryAddSingleton<IAssemblyCommandScanner, AssemblyCommandScanner>();

        return services;
    }
}

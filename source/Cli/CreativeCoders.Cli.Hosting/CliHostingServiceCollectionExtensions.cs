using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Spectre.Console;

namespace CreativeCoders.Cli.Hosting;

public static class CliHostingServiceCollectionExtensions
{
    public static IServiceCollection AddCliHosting(this IServiceCollection services)
    {
        services.TryAddSingleton<ICommandInfoCreator, CommandInfoCreator>();
        services.TryAddSingleton<IAssemblyCommandScanner, AssemblyCommandScanner>();
        services.TryAddSingleton<ICliCommandStore, CliCommandStore>();
        services.TryAddSingleton<ICliHost, DefaultCliHost>();
        services.TryAddSingleton<IAnsiConsole>(_ => AnsiConsole.Console);

        return services;
    }
}

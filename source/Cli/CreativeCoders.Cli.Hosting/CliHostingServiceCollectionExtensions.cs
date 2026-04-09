using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;
using CreativeCoders.Cli.Hosting.Commands.Validation;
using CreativeCoders.SysConsole.Cli.Parsing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Spectre.Console;

namespace CreativeCoders.Cli.Hosting;

/// <summary>
/// Provides extension methods for registering CLI hosting services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class CliHostingServiceCollectionExtensions
{
    /// <summary>
    /// Registers the required CLI hosting services in the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the CLI hosting services to.</param>
    public static void AddCliHosting(this IServiceCollection services)
    {
        services.TryAddSingleton<ICliCommandStructureValidator, CliCommandStructureValidator>();
        services.TryAddSingleton<ICommandInfoCreator, CommandInfoCreator>();
        services.TryAddSingleton<IAssemblyCommandScanner, AssemblyCommandScanner>();
        services.TryAddSingleton<ICliCommandStore, CliCommandStore>();
        services.TryAddSingleton<ICliHost, DefaultCliHost>();
        services.TryAddSingleton<IAnsiConsole>(_ => AnsiConsole.Console);
        services.AddOptionsHelpGenerator();
    }
}

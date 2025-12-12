using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;
using CreativeCoders.Cli.Hosting.Commands.Validation;
using CreativeCoders.SysConsole.Cli.Parsing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Spectre.Console;

namespace CreativeCoders.Cli.Hosting;

public static class CliHostingServiceCollectionExtensions
{
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

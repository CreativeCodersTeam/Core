using CreativeCoders.SysConsole.Cli.Parsing.Help;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.SysConsole.Cli.Parsing;

public static class ServiceCollectionExtensions
{
    public static void AddOptionsHelpGenerator(this IServiceCollection services)
    {
        services.TryAddSingleton<IOptionsHelpGenerator, OptionsHelpGenerator>();
    }
}

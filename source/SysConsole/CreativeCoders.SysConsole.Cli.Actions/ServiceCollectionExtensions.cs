using CreativeCoders.SysConsole.Cli.Actions.Routing;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.SysConsole.Cli.Actions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCliActions(this IServiceCollection services)
        {
            services.TryAddSingleton<ICliActionRuntimeBuilder, CliActionRuntimeBuilder>();

            services.TryAddSingleton<IRoutesBuilder, RoutesBuilder>();

            services.TryAddSingleton<ICliActionRouter, CliActionRouter>();
        }
    }
}

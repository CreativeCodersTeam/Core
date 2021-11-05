using CreativeCoders.SysConsole.Cli.Actions.Routing;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.SysConsole.Cli.Actions
{
    /// <summary>   A service collection extensions. </summary>
    public static class ServiceCollectionExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   An IServiceCollection extension method that adds the services needed for CLI actions. </summary>
        ///
        /// <param name="services"> The service collection to act on. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void AddCliActions(this IServiceCollection services)
        {
            services.TryAddSingleton<ICliActionRuntimeBuilder, CliActionRuntimeBuilder>();

            services.TryAddSingleton<IRoutesBuilder, RoutesBuilder>();

            services.TryAddSingleton<ICliActionRouter, CliActionRouter>();
        }
    }
}

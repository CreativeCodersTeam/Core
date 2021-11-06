using System;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.App;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.SysConsole.Cli.Actions
{
    /// <summary>   A console application builder extensions. </summary>
    public static class ConsoleAppBuilderExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Setups the ConsoleAppBuilder to use CLI actions for execution. </summary>
        ///
        /// <typeparam name="TCliStartup">  Type of the CLI startup. </typeparam>
        /// <param name="consoleAppBuilder">    The consoleAppBuilder to act on. </param>
        ///
        /// <returns>   A ConsoleAppBuilder. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static ConsoleAppBuilder UseActions<TCliStartup>(this ConsoleAppBuilder consoleAppBuilder)
            where TCliStartup : class, ICliStartup, new()
        {
            return consoleAppBuilder
                .UseStartup<TCliStartup>()
                .ConfigureServices(services => services.AddCliActions())
                .UseExecutor(sp =>
                {
                    var runtimeBuilder = sp.GetRequiredService<ICliActionRuntimeBuilder>();

                    var startup = sp.GetRequiredService<IStartup>() as ICliStartup;

                    startup?.Configure(runtimeBuilder);

                    var runtime = runtimeBuilder.Build();

                    return new CliActionsExecutor(runtime);
                });
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Setups the ConsoleAppBuilder to use CLI actions for execution. </summary>
        ///
        /// <typeparam name="TCliStartup">  Type of the CLI startup. </typeparam>
        /// <param name="consoleAppBuilder">    The consoleAppBuilder to act on. </param>
        /// <param name="setupRuntime">         Action for setting up the runtime. </param>
        ///
        /// <returns>   A ConsoleAppBuilder. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static ConsoleAppBuilder UseActions<TCliStartup>(this ConsoleAppBuilder consoleAppBuilder,
            Action<ICliActionRuntimeBuilder> setupRuntime)
            where TCliStartup : class, ICliStartup, new()
        {
            Ensure.NotNull(setupRuntime, nameof(setupRuntime));

            return consoleAppBuilder
                .UseStartup<TCliStartup>()
                .ConfigureServices(services => services.AddCliActions())
                .UseExecutor(sp =>
                {
                    var runtimeBuilder = sp.GetRequiredService<ICliActionRuntimeBuilder>();

                    var startup = sp.GetRequiredService<IStartup>() as ICliStartup;

                    setupRuntime(runtimeBuilder);

                    startup?.Configure(runtimeBuilder);

                    var runtime = runtimeBuilder.Build();

                    return new CliActionsExecutor(runtime);
                });
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Setups the ConsoleAppBuilder to use CLI actions for execution. </summary>
        ///
        /// <typeparam name="TStartup"> Type of the startup. </typeparam>
        /// <param name="consoleAppBuilder">    The consoleAppBuilder to act on. </param>
        /// <param name="setupRuntime">         Action for setting up the runtime. </param>
        ///
        /// <returns>   A ConsoleAppBuilder. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static ConsoleAppBuilder UseSimpleActions<TStartup>(this ConsoleAppBuilder consoleAppBuilder,
            Action<ICliActionRuntimeBuilder> setupRuntime)
            where TStartup : class, IStartup, new()
        {
            Ensure.NotNull(setupRuntime, nameof(setupRuntime));

            return consoleAppBuilder
                .UseStartup<TStartup>()
                .ConfigureServices(services => services.AddCliActions())
                .UseExecutor(sp =>
                {
                    var runtimeBuilder = sp.GetRequiredService<ICliActionRuntimeBuilder>();

                    setupRuntime(runtimeBuilder);

                    var runtime = runtimeBuilder.Build();

                    return new CliActionsExecutor(runtime);
                });
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Setups the ConsoleAppBuilder to use CLI actions for execution. </summary>
        ///
        /// <param name="consoleAppBuilder">    The consoleAppBuilder to act on. </param>
        /// <param name="setupRuntime">         Action for setting up the runtime. </param>
        ///
        /// <returns>   A ConsoleAppBuilder. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static ConsoleAppBuilder UseActions(this ConsoleAppBuilder consoleAppBuilder,
            Action<ICliActionRuntimeBuilder> setupRuntime)
        {
            Ensure.NotNull(setupRuntime, nameof(setupRuntime));

            return consoleAppBuilder
                .ConfigureServices(services => services.AddCliActions())
                .UseExecutor(sp =>
                {
                    var runtimeBuilder = sp.GetRequiredService<ICliActionRuntimeBuilder>();

                    setupRuntime(runtimeBuilder);

                    var runtime = runtimeBuilder.Build();

                    return new CliActionsExecutor(runtime);
                });
        }
    }
}

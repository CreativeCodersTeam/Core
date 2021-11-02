using System;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.App;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.SysConsole.Cli.Actions
{
    public static class ConsoleAppBuilderExtensions
    {
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

                    return new CliActionExecutor(runtime);
                });
        }

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

                    return new CliActionExecutor(runtime);
                });
        }

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

                    return new CliActionExecutor(runtime);
                });
        }

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

                    return new CliActionExecutor(runtime);
                });
        }
    }
}

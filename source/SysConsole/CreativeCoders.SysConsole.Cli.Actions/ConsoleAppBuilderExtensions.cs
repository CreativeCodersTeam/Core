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

        
    }
}

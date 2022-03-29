using CreativeCoders.SysConsole.Cli.Actions.Runtime;
using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;

public class Startup : ICliStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
    }

    public void Configure(ICliActionRuntimeBuilder runtimeBuilder)
    {
        runtimeBuilder.AddController<ConsoleAppTestController>();

        runtimeBuilder.UseRouting();
    }
}
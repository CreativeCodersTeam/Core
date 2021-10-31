using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.App;
using CreativeCoders.SysConsole.Cli.Actions.Definition;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;
using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests
{
    public class ConsoleAppBuilderTests
    {
        [Fact]
        public async Task Test()
        {
            var args = new[] {"test", "run"};

            var consoleApp = new ConsoleAppBuilder(args)
                .UseActions<Startup>()
                .Build();

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(ConsoleAppTestController.RunReturnCode);
        }
    }

    public class Startup : ICliStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
        }

        public void Configure(ICliActionRuntimeBuilder runtimeBuilder)
        {
            runtimeBuilder.AddController<ConsoleAppTestController>();
            //runtimeBuilder.UseMiddleware<CliRoutingMiddleware>();
        }
    }

    [UsedImplicitly]
    [CliController("test")]
    public class ConsoleAppTestController
    {
        public const int RunReturnCode = 13579;

        [CliAction("run")]
        public Task<int> RunAsync()
        {
            return Task.FromResult(RunReturnCode);
        }
    }
}

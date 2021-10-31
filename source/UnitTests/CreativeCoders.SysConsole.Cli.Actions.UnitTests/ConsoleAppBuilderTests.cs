using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.App;
using CreativeCoders.SysConsole.Cli.Actions.Definition;
using CreativeCoders.SysConsole.Cli.Actions.Exceptions;
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
        public async Task RunAsync_ConsoleAppControllerRun_ReturnsCorrectReturnCode()
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

        [Fact]
        public async Task RunAsync_ConsoleAppControllerDo_ReturnsCorrectReturnCode()
        {
            var args = new[] { "test", "do" };

            var consoleApp = new ConsoleAppBuilder(args)
                .UseActions<Startup>()
                .Build();

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(ConsoleAppTestController.DoReturnCode);
        }

        [Fact]
        public async Task RunAsync_ConsoleAppControllerAmbiguousAction_ThrowsException()
        {
            var args = new[] { "test", "do_this" };

            var consoleApp = new ConsoleAppBuilder(args)
                .UseActions<Startup>()
                .Build();

            // Act
            Func<Task> act = async () => await consoleApp.ReThrowExceptions(true).RunAsync();

            // Assert
            await act
                .Should()
                .ThrowAsync<AmbiguousRouteException>();
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

            runtimeBuilder.UseMiddleware<CliRoutingMiddleware>();
        }
    }

    [PublicAPI]
    [CliController("test")]
    public class ConsoleAppTestController
    {
        public const int RunReturnCode = 13579;

        public const int DoReturnCode = 2468;

        [CliAction("run")]
        public Task<CliActionResult> RunAsync()
        {
            return Task.FromResult(new CliActionResult(){ReturnCode = RunReturnCode});
        }

        [CliAction("do")]
        public Task<CliActionResult> DoAsync()
        {
            return Task.FromResult(new CliActionResult() { ReturnCode = DoReturnCode });
        }

        [CliAction("do_this")]
        public Task<CliActionResult> DoThis1Async()
        {
            return Task.FromResult(new CliActionResult() { ReturnCode = DoReturnCode });
        }

        [CliAction("do_this")]
        public Task<CliActionResult> DoThis2Async()
        {
            return Task.FromResult(new CliActionResult() { ReturnCode = DoReturnCode });
        }
    }
}

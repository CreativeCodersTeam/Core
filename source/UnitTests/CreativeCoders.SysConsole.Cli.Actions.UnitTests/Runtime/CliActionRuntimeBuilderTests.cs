using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;
using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.Runtime
{
    public class CliActionRuntimeBuilderTests
    {
        [Fact]
        public async Task Test()
        {
            var args = new[] {"test"};

            var builder = new CliActionRuntimeBuilder();

            var runtime = builder
                .UseMiddleware<FirstTestMiddleware>()
                .UseMiddleware<SecondTestMiddleware>()
                .Build();

            // Act
            var result = await runtime.ExecuteAsync(args);

            // Assert
            FirstTestMiddleware.IsCalled
                .Should()
                .BeTrue();

            SecondTestMiddleware.IsCalled
                .Should()
                .BeTrue();

            result
                .Should()
                .Be(SecondTestMiddleware.ReturnCode);
        }
    }

    [UsedImplicitly]
    public class FirstTestMiddleware : CliActionMiddlewareBase
    {
        public const int ReturnCode = 1357;

        public FirstTestMiddleware([NotNull] Func<CliActionContext, Task> next) : base(next) { }

        public override async Task InvokeAsync(CliActionContext context)
        {
            context.ReturnCode = ReturnCode;

            IsCalled = true;

            await Next(context);
        }

        public static bool IsCalled { get; set; }
    }

    [UsedImplicitly]
    public class SecondTestMiddleware : CliActionMiddlewareBase
    {
        public const int ReturnCode = 2345;

        public SecondTestMiddleware([NotNull] Func<CliActionContext, Task> next) : base(next) { }

        public override async Task InvokeAsync(CliActionContext context)
        {
            if (context.ReturnCode == FirstTestMiddleware.ReturnCode)
            {
                context.ReturnCode = ReturnCode;
            }

            IsCalled = true;

            await Next(context);
        }

        public static bool IsCalled { get; set; }
    }
}

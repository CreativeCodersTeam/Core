using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;
using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData
{
    [UsedImplicitly]
    public class FirstTestMiddleware : CliActionMiddlewareBase
    {
        public const int ReturnCode = 1357;

        public FirstTestMiddleware(Func<CliActionContext, Task> next) : base(next) { }

        public override async Task InvokeAsync(CliActionContext context)
        {
            context.ReturnCode = ReturnCode;

            IsCalled = true;

            await Next(context);
        }

        public static bool IsCalled { get; private set; }
    }
}

using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;
using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;

[UsedImplicitly]
public class SecondTestMiddleware : CliActionMiddlewareBase
{
    public const int ReturnCode = 2345;

    public SecondTestMiddleware(Func<CliActionContext, Task> next) : base(next) { }

    public override async Task InvokeAsync(CliActionContext context)
    {
        if (context.ReturnCode == FirstTestMiddleware.ReturnCode)
        {
            context.ReturnCode = ReturnCode;
        }

        IsCalled = true;

        await Next(context);
    }

    public static bool IsCalled { get; private set; }
}
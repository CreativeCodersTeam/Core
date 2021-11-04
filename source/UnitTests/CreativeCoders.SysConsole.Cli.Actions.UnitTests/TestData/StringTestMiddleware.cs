using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;
using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData
{
    [UsedImplicitly]
    public class StringTestMiddleware : CliActionMiddlewareBase
    {
        private readonly string _text;

        public StringTestMiddleware(Func<CliActionContext, Task> next, string text) : base(next)
        {
            _text = text;
        }

        public override Task InvokeAsync(CliActionContext context)
        {
            context.ReturnCode = _text?.GetHashCode() ?? 0;
            return Task.CompletedTask;
        }
    }
}

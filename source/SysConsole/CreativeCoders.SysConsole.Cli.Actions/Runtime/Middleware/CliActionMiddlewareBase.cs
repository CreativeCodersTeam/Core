using System;
using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware
{
    public abstract class CliActionMiddlewareBase
    {
        protected CliActionMiddlewareBase(Func<CliActionContext, Task> next)
        {
            Next = next;
        }

        public abstract Task InvokeAsync(CliActionContext context);

        protected Func<CliActionContext, Task> Next { get; }
    }
}

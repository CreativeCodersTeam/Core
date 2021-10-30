using System;
using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.Cli.Actions.Execution
{
    public abstract class CliActionExecutionMiddlewareBase
    {
        protected CliActionExecutionMiddlewareBase(Func<CliActionContext, Task> next)
        {
            Next = next;
        }

        public abstract Task InvokeAsync(CliActionContext context);

        protected Func<CliActionContext, Task> Next { get; }
    }
}

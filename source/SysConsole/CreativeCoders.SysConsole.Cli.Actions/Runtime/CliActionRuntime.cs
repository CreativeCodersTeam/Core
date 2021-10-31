using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.Cli.Actions.Routing;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    public class CliActionRuntime : ICliActionRuntime
    {
        private Func<CliActionContext, Task>? _middlewarePipeline;

        public void Init(Func<Func<CliActionContext, Task>, Func<CliActionContext, Task>> createPipeline,
            IEnumerable<CliActionRoute> actionRoutes)
        {
            _middlewarePipeline = createPipeline(ExecuteAction);
        }

        private Task ExecuteAction(CliActionContext context)
        {
            context.ReturnCode = int.MinValue;

            return Task.CompletedTask;
        }

        public async Task<int> ExecuteAsync(string[] args)
        {
            if (_middlewarePipeline == null)
            {
                throw new InvalidOperationException("CLI action runtime not initialized");
            }

            var context = new CliActionContext(new CliActionRequest(args));

            await _middlewarePipeline(context);

            return context.ReturnCode;
        }
    }
}

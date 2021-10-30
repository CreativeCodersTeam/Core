using System;
using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    public class CliActionRuntime : ICliActionRuntime
    {
        private readonly Func<CliActionContext, Task> _middlewarePipeline;

        public CliActionRuntime(Func<CliActionContext, Task> middlewarePipeline)
        {
            _middlewarePipeline = middlewarePipeline;
        }

        public async Task<int> ExecuteAsync(string[] args)
        {
            var context = new CliActionContext(new CliActionRequest(args));

            await _middlewarePipeline(context);

            return context.ReturnCode;
        }
    }
}

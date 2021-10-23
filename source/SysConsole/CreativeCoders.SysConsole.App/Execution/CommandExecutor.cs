using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core;

namespace CreativeCoders.SysConsole.App.Execution
{
    internal class CommandExecutor : ICommandExecutor
    {
        private readonly IExecutor[] _executors;

        public CommandExecutor(IExecutorChain executorChain)
        {
            Ensure.NotNull(executorChain, nameof(executorChain));

            _executors = executorChain.GetExecutors().ToArray();
        }

        public async Task<int> Execute(string[] args)
        {
            foreach (var executor in _executors)
            {
                var result = await executor.TryExecuteAsync(args);

                if (result.ExecutionIsHandled)
                {
                    return result.ReturnCode;
                }
            }

            return int.MinValue;
        }
    }
}

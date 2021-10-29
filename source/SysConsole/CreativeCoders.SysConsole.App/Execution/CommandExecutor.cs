using System;
using System.Threading.Tasks;
using CreativeCoders.Core;

namespace CreativeCoders.SysConsole.App.Execution
{
    internal class CommandExecutor : ICommandExecutor
    {
        private readonly Func<string[], Task<int>> _executeAsync;
        
        public CommandExecutor(Func<string[], Task<int>> executeAsync)
        {
            _executeAsync = Ensure.NotNull(executeAsync, nameof(executeAsync));
        }

        public async Task<int> ExecuteAsync(string[] args)
        {
            return await _executeAsync(args).ConfigureAwait(false);
        }
    }
}

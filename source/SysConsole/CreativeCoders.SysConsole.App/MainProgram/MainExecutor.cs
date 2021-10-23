using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.App.Execution;

namespace CreativeCoders.SysConsole.App.MainProgram
{
    public class MainExecutor : IExecutor
    {
        private readonly IMain _main;

        public MainExecutor(IMain main)
        {
            _main = Ensure.NotNull(main, nameof(main));
        }

        public async Task<ExecutorResult> TryExecuteAsync(string[] args)
        {
            return new ExecutorResult(true, await _main.ExecuteAsync(args).ConfigureAwait(false));
        }
    }
}

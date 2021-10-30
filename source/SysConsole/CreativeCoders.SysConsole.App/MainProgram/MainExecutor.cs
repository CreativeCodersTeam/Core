using System.Threading.Tasks;
using CreativeCoders.Core;

namespace CreativeCoders.SysConsole.App.MainProgram
{
    public class MainExecutor : ICommandExecutor
    {
        private readonly IMain _main;

        public MainExecutor(IMain main)
        {
            _main = Ensure.NotNull(main, nameof(main));
        }

        public async Task<int> ExecuteAsync(string[] args)
        {
            return await _main.ExecuteAsync(args).ConfigureAwait(false);
        }
    }
}

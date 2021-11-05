using System.Threading.Tasks;
using CreativeCoders.SysConsole.App;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;

namespace CreativeCoders.SysConsole.Cli.Actions
{
    internal class CliActionExecutor : IConsoleAppExecutor
    {
        private readonly ICliActionRuntime _runtime;

        public CliActionExecutor(ICliActionRuntime runtime)
        {
            _runtime = runtime;
        }

        public Task<int> ExecuteAsync(string[] args)
        {
            return _runtime.ExecuteAsync(args);
        }
    }
}

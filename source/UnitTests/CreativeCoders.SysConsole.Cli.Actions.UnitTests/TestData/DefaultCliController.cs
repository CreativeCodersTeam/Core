using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Definition;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData
{
    [CliController]
    public class DefaultCliController
    {
        [CliAction]
        public Task<CliActionResult> DoDefaultAsync()
        {
            return Task.FromResult(new CliActionResult());
        }

        [CliAction("command")]
        public Task<CliActionResult> DoCommandAsync()
        {
            return Task.FromResult(new CliActionResult());
        }
    }
}

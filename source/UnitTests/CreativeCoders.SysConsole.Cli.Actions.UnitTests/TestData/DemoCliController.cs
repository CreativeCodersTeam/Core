using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Definition;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData
{
    [CliController("demo")]
    public class DemoCliController
    {
        [CliAction("test")]
        public Task<CliActionResult> DoAsync(DoCmdOptions options)
        {
            return Task.FromResult<CliActionResult>(null);
        }

        [CliAction("more/command")]
        public Task<CliActionResult> DoMoreAsync(DoCmdOptions options)
        {
            return Task.FromResult<CliActionResult>(null);
        }
    }
}

using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Definition;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData
{
    [CliController("demo")]
    public class DemoCliController
    {
        [CliAction]
        public Task<CliActionResult> DoAsync(DoCmdOptions options)
        {
            return Task.FromResult<CliActionResult>(null);
        }
    }
}

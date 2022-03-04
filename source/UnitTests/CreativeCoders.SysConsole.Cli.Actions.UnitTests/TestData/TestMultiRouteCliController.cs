using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Definition;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData
{
    [CliController]
    [CliController("controller")]
    public class TestMultiRouteCliController
    {
        public const int ExecuteReturnCode = 13579;

        [CliAction("do")]
        public Task<CliActionResult> DoAsync()
        {
            return Task.FromResult(new CliActionResult(-1));
        }

        [CliAction]
        [CliAction("execute")]
        public Task<CliActionResult> ExecuteAsync(DoCmdOptions options)
        {
            if (string.IsNullOrEmpty(options.Text))
            {
                return Task.FromResult(new CliActionResult(ExecuteReturnCode));
            }

            return Task.FromResult(new CliActionResult(options.Text.GetHashCode()));
        }
    }
}

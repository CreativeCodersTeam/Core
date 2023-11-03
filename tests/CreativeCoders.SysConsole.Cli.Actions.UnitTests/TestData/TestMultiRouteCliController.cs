using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Definition;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;

[PublicAPI]
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
        return Task.FromResult(
            string.IsNullOrEmpty(options.Text)
                ? new CliActionResult(ExecuteReturnCode)
                : new CliActionResult(options.Text.GetHashCode()));
    }
}

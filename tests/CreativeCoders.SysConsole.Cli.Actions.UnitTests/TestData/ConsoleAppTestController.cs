using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Definition;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;

[PublicAPI]
[CliController("test")]
public class ConsoleAppTestController
{
    public const int RunReturnCode = 13579;

    public const int DoReturnCode = 2468;

    [CliAction("run")]
    public Task<CliActionResult> RunAsync()
    {
        return Task.FromResult(new CliActionResult { ReturnCode = RunReturnCode });
    }

    [CliAction("do", AlternativeRoute = ["start", "this", "action"])]
    public Task<CliActionResult> DoAsync()
    {
        return Task.FromResult(new CliActionResult { ReturnCode = DoReturnCode });
    }

    [CliAction("do_this")]
    public Task<CliActionResult> DoThis1Async()
    {
        return Task.FromResult(new CliActionResult { ReturnCode = DoReturnCode });
    }

    [CliAction("do_this")]
    public Task<CliActionResult> DoThis2Async()
    {
        return Task.FromResult(new CliActionResult { ReturnCode = DoReturnCode });
    }
}

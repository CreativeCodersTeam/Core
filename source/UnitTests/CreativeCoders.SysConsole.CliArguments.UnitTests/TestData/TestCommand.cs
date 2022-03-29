using System.Threading.Tasks;
using CreativeCoders.SysConsole.CliArguments.Commands;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.TestData;

[UsedImplicitly]
public class TestCommand : CliCommandBase<TestCommandOptions>
{
    public const int ReturnCode = 1357;

    public override Task<CliCommandResult> ExecuteAsync(TestCommandOptions options)
    {
        return Task.FromResult(new CliCommandResult(ReturnCode));
    }

    public override string Name { get; set; } = "command";
}
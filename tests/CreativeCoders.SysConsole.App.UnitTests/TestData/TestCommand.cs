using System.Threading.Tasks;
using CreativeCoders.SysConsole.CliArguments.Commands;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData;

[UsedImplicitly]
public class TestCommand : CliCommandBase<TestCommandOptions>
{
    public const int ReturnCode = 123;

    public override Task<CliCommandResult> ExecuteAsync(TestCommandOptions options)
    {
        OptionsFirstArg = options.FirstArg;

        return Task.FromResult(new CliCommandResult(ReturnCode));
    }

    public override string Name { get; set; } = "test";

    public static string? OptionsFirstArg { get; private set; }
}

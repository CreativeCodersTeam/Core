using System.Threading.Tasks;
using CreativeCoders.SysConsole.CliArguments.Commands;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData
{
    [UsedImplicitly]
    public class TestFallbackCommand : CliCommandBase<TestCommandOptions>
    {
        public const int ReturnCode = 2468;

        public override Task<CliCommandResult> ExecuteAsync(TestCommandOptions options)
        {
            return Task.FromResult(new CliCommandResult(ReturnCode));
        }

        public override string Name { get; set; } = "test";
    }
}

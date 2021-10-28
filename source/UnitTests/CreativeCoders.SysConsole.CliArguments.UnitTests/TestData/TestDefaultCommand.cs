using System.Threading.Tasks;
using CreativeCoders.SysConsole.CliArguments.Commands;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.TestData
{
    [UsedImplicitly]
    public class TestDefaultCommand : CliCommandBase<TestCommandOptions>
    {
        public const int ReturnCode = 13579;

        public TestDefaultCommand()
        {
            IsDefault = true;
        }

        public override Task<CliCommandResult> ExecuteAsync(TestCommandOptions options)
        {
            return Task.FromResult(new CliCommandResult(ReturnCode));
        }

        public override string Name { get; set; } = "command";
    }
}

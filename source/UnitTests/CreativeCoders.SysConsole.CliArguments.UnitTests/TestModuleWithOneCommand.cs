using System.Threading.Tasks;
using CreativeCoders.SysConsole.CliArguments.Building;
using CreativeCoders.SysConsole.CliArguments.Commands;
using CreativeCoders.SysConsole.CliArguments.UnitTests.TestData;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests
{
    [UsedImplicitly]
    public class TestModuleWithOneCommand : ICliModule
    {
        public const int ReturnCode = 1234;

        public void Configure(ICliBuilder cliBuilder)
        {
            cliBuilder.AddCommand<TestCommandOptions>("command",
                _ => Task.FromResult(new CliCommandResult(ReturnCode)));
        }
    }
}

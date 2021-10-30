using CreativeCoders.SysConsole.CliArguments.Options;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData
{
    [PublicAPI]
    public class TestCommandOptions
    {
        [OptionValue(0)]
        public string? FirstArg { get; set; }
    }
}

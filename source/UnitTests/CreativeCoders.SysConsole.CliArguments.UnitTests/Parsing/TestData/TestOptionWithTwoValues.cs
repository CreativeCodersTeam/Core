using CreativeCoders.SysConsole.Cli.Parsing;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing.TestData
{
    public class TestOptionWithTwoValues
    {
        [OptionValue(0)]
        public string? FirstValue { get; [UsedImplicitly] set; }

        [OptionValue(1, DefaultValue = "Fallback")]
        public string? SecondValue { get; [UsedImplicitly] set; }
    }
}

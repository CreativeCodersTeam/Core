using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData
{
    [PublicAPI]
    [Options(AllArgsMustMatch = true)]
    public class TestOptionAllArgsMustMatch
    {
        [OptionParameter('f', "first")]
        public string? FirstValue { get; set; }

        [OptionParameter('s', "second")]
        public string? SecondValue { get; set; }
    }
}

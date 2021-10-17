using CommandLine;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData
{
    [Verb("test")]
    public class TestVerbOptions
    {
        [Value(0)]
        public string? FirstArg { get; set; }
    }
}

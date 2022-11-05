using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData;

[PublicAPI]
public class TestOptionWithSingleBool
{
    [OptionParameter('c', "create")]
    public bool CreateData { get; set; }
}

using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData;

[UsedImplicitly]
public class TestOptionWithNullable
{
    [OptionParameter('n', "nullable")] public int? NullableIntValue { get; set; }

    [OptionParameter('b', "bool")] public bool? NullableBoolValue { get; set; }
}

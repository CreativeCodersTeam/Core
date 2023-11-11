#nullable enable
using JetBrains.Annotations;

namespace CreativeCoders.Core.UnitTests.Reflection.TestData;

public class TestSimpleClassOptions : ITestSimpleClassOptions
{
    [UsedImplicitly]
    public string? Value { get; set; }
}

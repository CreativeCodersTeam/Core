#nullable enable
using JetBrains.Annotations;

namespace CreativeCoders.Core.UnitTests;

[PublicAPI]
public class UnitTestDemoObject
{
    public string? Text { get; set; }

    public int IntValue { get; set; }

    public bool BoolValue { get; set; }
}

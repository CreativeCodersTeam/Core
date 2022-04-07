using JetBrains.Annotations;

namespace CreativeCoders.Core.UnitTests;

public interface ITestIntf
{
    int IntValue { get; }
}

[UsedImplicitly]
public class TestIntfClass : ITestIntf
{
    public int IntValue => 11;
}

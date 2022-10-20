using JetBrains.Annotations;

namespace CreativeCoders.Core.UnitTests;

[PublicAPI]
public interface ITestIntf
{
    int IntValue { get; }
}

[PublicAPI]
public class TestIntfClass : ITestIntf
{
    public int IntValue => 11;
}

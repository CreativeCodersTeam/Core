using JetBrains.Annotations;

namespace CreativeCoders.DependencyInjection.UnitTests.TestData;

[UsedImplicitly]
public class TestClassWithStringParam
{
    public TestClassWithStringParam(string text)
    {
        Text = text;
    }

    public string Text { get; }
}

#nullable enable
namespace CreativeCoders.Core.UnitTests.Reflection.TestData;

public class TestSimpleClass
{
    public TestSimpleClass(string text)
    {
        Text = text;
    }

    public string Text { get; }
}
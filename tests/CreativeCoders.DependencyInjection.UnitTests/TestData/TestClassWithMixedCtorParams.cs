using JetBrains.Annotations;

namespace CreativeCoders.DependencyInjection.UnitTests.TestData;

[UsedImplicitly]
public class TestClassWithMixedCtorParams
{
    public TestClassWithMixedCtorParams(string text, ITestService testService)
    {
        Text = text;
        TestService = testService;
    }

    public string Text { get; }

    public ITestService TestService { get; }
}

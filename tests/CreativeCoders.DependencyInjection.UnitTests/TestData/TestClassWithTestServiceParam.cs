using JetBrains.Annotations;

namespace CreativeCoders.DependencyInjection.UnitTests.TestData;

[UsedImplicitly]
public class TestClassWithTestServiceParam
{
    public TestClassWithTestServiceParam(ITestService testService)
    {
        TestService = testService;
    }

    public ITestService TestService { get; }
}

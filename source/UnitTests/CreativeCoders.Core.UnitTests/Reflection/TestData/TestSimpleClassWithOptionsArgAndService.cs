#nullable enable
namespace CreativeCoders.Core.UnitTests.Reflection.TestData
{
    public class TestSimpleClassWithOptionsArgAndService
    {
        public TestSimpleClassWithOptionsArgAndService(TestSimpleClassOptions options, ITestService testService)
        {
            Options = options;
            TestService = testService;
        }

        public TestSimpleClassOptions Options { get; }

        public ITestService TestService { get; }
    }
}

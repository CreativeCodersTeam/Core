namespace CreativeCoders.Core.UnitTests.Di.Helper
{
    public class TestClassForExecutor
    {
        public void DoSomething(ITestService testService, IUnknownService unknownService, ITestService<int> testServiceInt)
        {
            TestService = testService;
            UnknownService = unknownService;
            TestServiceInt = testServiceInt;
        }
        public string DoSomethingWithResult(ITestService testService, IUnknownService unknownService, ITestService<int> testServiceInt)
        {
            TestService = testService;
            UnknownService = unknownService;
            TestServiceInt = testServiceInt;

            return nameof(DoSomethingWithResult);
        }

        public ITestService<int> TestServiceInt { get; private set; }

        public IUnknownService UnknownService { get; private set; }

        public ITestService TestService { get; private set; }
    }
}
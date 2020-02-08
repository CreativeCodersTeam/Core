using CreativeCoders.Core.UnitTests.Di.Helper;
using CreativeCoders.Di;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Di
{
    public class MethodExecutorTests
    {
        [Fact]
        public void Execute_CallMethodWithoutResult_AllParametersResolved()
        {
            var testService = new TestService();
            var unknownService = new UnknownService();
            var testServiceInt = new TestService<int>();

            var container = CreateContainerMock(testService, unknownService, testServiceInt);

            var instance = new TestClassForExecutor();

            MethodExecutor.Execute(container, instance, instance.GetType().GetMethod(nameof(TestClassForExecutor.DoSomething)));

            Assert.Same(testService, instance.TestService);
            Assert.Same(unknownService, instance.UnknownService);
            Assert.Same(testServiceInt, instance.TestServiceInt);
        }

        [Fact]
        public void Execute_CallMethodWithResult_AllParametersResolved()
        {
            var testService = new TestService();
            var unknownService = new UnknownService();
            var testServiceInt = new TestService<int>();

            var container = CreateContainerMock(testService, unknownService, testServiceInt);

            var instance = new TestClassForExecutor();

            var result = MethodExecutor.Execute<string>(container, instance,
                instance
                .GetType()
                .GetMethod(nameof(TestClassForExecutor.DoSomethingWithResult)));

            Assert.Same(testService, instance.TestService);
            Assert.Same(unknownService, instance.UnknownService);
            Assert.Same(testServiceInt, instance.TestServiceInt);

            Assert.Equal(nameof(TestClassForExecutor.DoSomethingWithResult), result);
        }

        private static IDiContainer CreateContainerMock(ITestService testService, IUnknownService unknownService, ITestService<int> testServiceInt)
        {
            var container = A.Fake<IDiContainer>();

            A.CallTo(() => container.GetInstance(typeof(ITestService))).Returns(testService);
            A.CallTo(() => container.GetInstance(typeof(IUnknownService))).Returns(unknownService);
            A.CallTo(() => container.GetInstance(typeof(ITestService<int>))).Returns(testServiceInt);

            return container;
        }
    }
}
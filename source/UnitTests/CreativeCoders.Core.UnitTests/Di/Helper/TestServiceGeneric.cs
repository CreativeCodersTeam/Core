using CreativeCoders.Di.Building;

namespace CreativeCoders.Core.UnitTests.Di.Helper
{
    [Implements(typeof(ITestService<>), Lifecycle = ImplementationLifecycle.Singleton)]
    public class TestService<T> : ITestService<T>
    {
        public T Data { get; set; }
    }
}
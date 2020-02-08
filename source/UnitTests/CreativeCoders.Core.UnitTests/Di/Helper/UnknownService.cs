using CreativeCoders.Di.Building;

namespace CreativeCoders.Core.UnitTests.Di.Helper
{
    [Implements(typeof(IUnknownService), Lifecycle = ImplementationLifecycle.Scoped)]
    public class UnknownService : IUnknownService
    {
        
    }
}
using CreativeCoders.Di.Building;

namespace CreativeCoders.Core.UnitTests.Di.Helper;

[Implements(typeof(ITestService))]
public class TestService : ITestService
{
    public string Text { get; set; }
}

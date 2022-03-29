using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Core.UnitTests.Di.Helper;

[UsedImplicitly]
public class TestServiceContainer
{
    public TestServiceContainer(IEnumerable<ITestService> services)
    {
        Services = services;
    }

    public IEnumerable<ITestService> Services { get; }
}
using System;
using CreativeCoders.Core.UnitTests.Di.Helper;
using CreativeCoders.Di;
using CreativeCoders.Di.MsServiceProvider;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Di;

[Collection("ServiceLocator")]
public class MsServiceProviderTests
{
    [Fact]
    public void ServiceLocatorTests()
    {
#pragma warning disable 618
        ServiceLocator.XRemoveDiContainerForUnitTests();
#pragma warning restore 618

        Assert.Throws<ArgumentNullException>(() => new ServiceProviderDiContainer(null));

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<ITestService, TestService>();
        serviceCollection.AddTransient<object>();
        var diContainer = new ServiceProviderDiContainer(serviceCollection.BuildServiceProvider());
        ServiceLocator.Init(() => diContainer);

        var service0 = ServiceLocator.GetInstance<ITestService>();
        Assert.True(service0 is TestService);

        var obj0 = ServiceLocator.GetInstance(typeof(ITestService));
        Assert.True(obj0 is TestService);
    }
}
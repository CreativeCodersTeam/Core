using System.Linq;
using CreativeCoders.Core.UnitTests.Di.Helper;
using CreativeCoders.Di;
using CreativeCoders.Di.Exceptions;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Di;

[Collection("ServiceLocator")]
public class ServiceLocatorTests
{
    [Fact]
    public void ServiceLocatorNotInitializedTest()
    {
#pragma warning disable 618
        ServiceLocator.XRemoveDiContainerForUnitTests();
#pragma warning restore 618

        Assert.Throws<ServiceLocatorNotInitializedException>(ServiceLocator.GetInstance<ITestService>);
        Assert.Throws<ServiceLocatorNotInitializedException>(() => ServiceLocator.GetInstance(typeof(ITestService)));
    }

    [Fact]
    public void ServiceLocatorInitializedTest()
    {
#pragma warning disable 618
        ServiceLocator.XRemoveDiContainerForUnitTests();
#pragma warning restore 618

        var newInstance = new TestService();

        var diContainer = A.Fake<IDiContainer>();

        A.CallTo(() => diContainer.GetInstance<ITestService>()).Returns(newInstance);

        ServiceLocator.Init(() => diContainer);
        Assert.Throws<ServiceLocatorAlreadyInitializedException>(() => ServiceLocator.Init(() => diContainer));
        var instance = ServiceLocator.GetInstance<ITestService>();

        Assert.Same(newInstance, instance);
    }

    [Fact]
    public void GetInstances_TwoInstancesRegistered_ReturnsInstances()
    {
#pragma warning disable 618
        ServiceLocator.XRemoveDiContainerForUnitTests();
#pragma warning restore 618

        var newInstance = new TestService();
        var newInstance2 = new TestService();

        var diContainer = A.Fake<IDiContainer>();

        A.CallTo(() => diContainer.GetInstances<ITestService>()).Returns(new ITestService[]{newInstance, newInstance2});
        A.CallTo(() => diContainer.GetInstances(typeof(ITestService))).Returns(new ITestService[] { newInstance, newInstance2 });

        ServiceLocator.Init(() => diContainer);
        Assert.Throws<ServiceLocatorAlreadyInitializedException>(() => ServiceLocator.Init(() => diContainer));
        var instances = ServiceLocator.GetInstances<ITestService>().ToArray();

        Assert.Equal(2, instances.Length);
        Assert.Contains(instances, x => x == newInstance);
        Assert.Contains(instances, x => x == newInstance2);

        var instanceObjects = ServiceLocator.GetInstances(typeof(ITestService)).ToArray();

        Assert.Equal(2, instanceObjects.Length);
        Assert.Contains(instanceObjects, x => x == newInstance);
        Assert.Contains(instanceObjects, x => x == newInstance2);
    }
}
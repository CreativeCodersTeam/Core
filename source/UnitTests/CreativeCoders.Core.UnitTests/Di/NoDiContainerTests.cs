using CreativeCoders.Core.UnitTests.Di.Helper;
using CreativeCoders.Di;
using CreativeCoders.Di.Exceptions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Di;

public class NoDiContainerTests
{
    [Fact]
    public void ThrowExceptionsTest()
    {
        var diContainer = new NoDiContainer();
        Assert.Throws<ServiceLocatorNotInitializedException>(() => diContainer.GetInstance<ITestService>());
        Assert.Throws<ServiceLocatorNotInitializedException>(() =>
            diContainer.GetInstance(typeof(ITestService)));
        Assert.Throws<ServiceLocatorNotInitializedException>(() =>
            diContainer.GetService(typeof(ITestService)));
        Assert.Throws<ServiceLocatorNotInitializedException>(() => diContainer.GetInstances<ITestService>());
        Assert.Throws<ServiceLocatorNotInitializedException>(() =>
            diContainer.GetInstances(typeof(ITestService)));
        Assert.Throws<ServiceLocatorNotInitializedException>(() =>
            diContainer.GetInstance<ITestService>("2"));
        Assert.Throws<ServiceLocatorNotInitializedException>(() =>
            diContainer.CreateScope());
        Assert.Throws<ServiceLocatorNotInitializedException>(() => diContainer.TryGetInstance<ITestService>(out _));
        Assert.Throws<ServiceLocatorNotInitializedException>(() =>
            diContainer.TryGetInstance(typeof(ITestService), out _));
        Assert.Throws<ServiceLocatorNotInitializedException>(() =>
            diContainer.GetInstance(typeof(ITestService), "Test"));
    }
}
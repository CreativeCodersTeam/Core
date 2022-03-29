using System;
using CreativeCoders.Core.UnitTests.Di.Helper;
using CreativeCoders.Di.MsServiceProvider;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Di.MsServiceProvider;

public class ServiceProviderContainerTests
{
    [Fact]
    public void CtorTest()
    {
        Assert.Throws<ArgumentNullException>(() => new ServiceProviderDiContainer(null));

        // ReSharper disable once ObjectCreationAsStatement
        new ServiceProviderDiContainer(new ServiceCollection().BuildServiceProvider());
    }

    [Fact]
    public void GetInstanceTest()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<ITestService, TestService>();

        var container = new ServiceProviderDiContainer(serviceCollection.BuildServiceProvider());

        new DiContainerTestHelper().TestGetInstance<ITestService, TestService>(container);
    }

    [Fact]
    public void GetInstanceTestThrowsException()
    {
        var serviceCollection = new ServiceCollection();
            
        var container = new ServiceProviderDiContainer(serviceCollection.BuildServiceProvider());

        new DiContainerTestHelper().TestGetInstanceThrowsException<ITestService>(container);
    }

    [Fact]
    public void GetInstancesTest()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<ITestService, TestService>();
        serviceCollection.AddTransient<ITestService, TestServiceWithNoCtorParam>();

        var container = new ServiceProviderDiContainer(serviceCollection.BuildServiceProvider());

        new DiContainerTestHelper().TestGetInstances<ITestService>(container, typeof(TestService), typeof(TestServiceWithNoCtorParam));
    }

    [Fact]
    public void GetInstances_NoServicesFound_ReturnsEmptyArray()
    {
        var container = new ServiceProviderDiContainer(new ServiceCollection().BuildServiceProvider());

        new DiContainerTestHelper().GetInstances_NoServicesFound_ReturnsEmptyArray<ITestService>(container);
    }

    [Fact]
    public void TryGetInstance_NoServiceFound_ReturnsFalseAndNull()
    {
        var container = new ServiceProviderDiContainer(new ServiceCollection().BuildServiceProvider());

        new DiContainerTestHelper().TryGetInstance_NoServiceFound_ReturnsFalseAndNull<ITestService>(container);
    }

    [Fact]
    public void TryGetInstance_ServiceFound_ReturnsTrueAndInstance()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<ITestService, TestService2>();
        var container = new ServiceProviderDiContainer(serviceCollection.BuildServiceProvider());

        new DiContainerTestHelper().TryGetInstance_ServiceFound_ReturnsTrueAndInstance<ITestService>(container);
    }

    [Fact]
    public void CreateScopeTest()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ITestService, TestService>();

        var container = new ServiceProviderDiContainer(serviceCollection.BuildServiceProvider());

        new DiContainerTestHelper().TestCreateScope<ITestService, TestService>(container);

        new DiContainerTestHelper().TestScopeDisposeDouble(container);
    }
}
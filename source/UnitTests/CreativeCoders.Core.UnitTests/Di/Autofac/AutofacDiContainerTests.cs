using System;
using Autofac;
using Autofac.Features.ResolveAnything;
using CreativeCoders.Core.UnitTests.Di.Helper;
using CreativeCoders.Di.Autofac;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Di.Autofac;

public class AutofacDiContainerTests
{
    [Fact]
    public void CtorTest()
    {
        Assert.Throws<ArgumentNullException>(() => new AutofacDiContainer(null));

        var _ = new AutofacDiContainer(new ContainerBuilder().Build());
    }

    [Fact]
    public void GetInstanceTest()
    {
        var autofacContainerBuilder = new ContainerBuilder();
        autofacContainerBuilder.RegisterType<TestServiceWithNoCtorParam>().As<ITestService>();
        autofacContainerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

        var container = new AutofacDiContainer(autofacContainerBuilder.Build());

        new DiContainerTestHelper().TestGetInstance<ITestService, TestServiceWithNoCtorParam>(container);
    }

    [Fact]
    public void GetInstanceTestThrowsException()
    {
        var autofacContainerBuilder = new ContainerBuilder();

        var container = new AutofacDiContainer(autofacContainerBuilder.Build());

        new DiContainerTestHelper().TestGetInstanceThrowsException<ITestService>(container);
    }

    [Fact]
    public void GetInstancesTest()
    {
        var _ = new[] {typeof(TestServiceWithNoCtorParam).Assembly};

        var autofacContainerBuilder = new ContainerBuilder();
        autofacContainerBuilder.RegisterType<TestService2>().As<ITestService>();
        autofacContainerBuilder.RegisterType<TestServiceWithNoCtorParam>().As<ITestService>();

        var container = new AutofacDiContainer(autofacContainerBuilder.Build());

        new DiContainerTestHelper().TestGetInstances<ITestService>(container, typeof(TestService2),
            typeof(TestServiceWithNoCtorParam));
    }

    [Fact]
    public void GetInstances_NoServicesFound_ReturnsEmptyArray()
    {
        var autofacContainerBuilder = new ContainerBuilder();

        var container = new AutofacDiContainer(autofacContainerBuilder.Build());

        new DiContainerTestHelper().GetInstances_NoServicesFound_ReturnsEmptyArray<ITestService>(container);
    }

    [Fact]
    public void TryGetInstance_NoServiceFound_ReturnsFalseAndNull()
    {
        var autofacContainerBuilder = new ContainerBuilder();

        var container = new AutofacDiContainer(autofacContainerBuilder.Build());

        new DiContainerTestHelper()
            .TryGetInstance_NoServiceFound_ReturnsFalseAndNull<ITestService>(container);
    }

    [Fact]
    public void TryGetInstance_ServiceFound_ReturnsTrueAndInstance()
    {
        var autofacContainerBuilder = new ContainerBuilder();
        autofacContainerBuilder.RegisterType<TestService2>().As<ITestService>();
        var container = new AutofacDiContainer(autofacContainerBuilder.Build());

        new DiContainerTestHelper()
            .TryGetInstance_ServiceFound_ReturnsTrueAndInstance<ITestService>(container);
    }

    [Fact]
    public void CreateScopeTest()
    {
        var autofacContainerBuilder = new ContainerBuilder();
        autofacContainerBuilder.RegisterType<TestServiceWithNoCtorParam>().As<ITestService>()
            .InstancePerLifetimeScope();
        var container = new AutofacDiContainer(autofacContainerBuilder.Build());

        new DiContainerTestHelper().TestCreateScope<ITestService, TestServiceWithNoCtorParam>(container);

        new DiContainerTestHelper().TestScopeDisposeDouble(container);
    }
}

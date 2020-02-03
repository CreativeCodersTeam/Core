using System;
using CreativeCoders.Core.UnitTests.Di.Helper;
using CreativeCoders.Di.SimpleInjector;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Di.SimpleInjector
{
    public class SimpleInjectorContainerTests
    {
        [Fact]
        public void CtorTest()
        {
            Assert.Throws<ArgumentNullException>(() => new SimpleInjectorDiContainer(null));

            var _ = new SimpleInjectorDiContainer(new Container());
        }

        [Fact]
        public void GetInstanceTest()
        {
            var simpleInjectorContainer = new Container();
            simpleInjectorContainer.Register<ITestService, TestServiceWithNoCtorParam>();
            
            var container = new SimpleInjectorDiContainer(simpleInjectorContainer);

            new DiContainerTestHelper().TestGetInstance<ITestService, TestServiceWithNoCtorParam>(container);
        }

        [Fact]
        public void GetInstanceTestThrowsException()
        {
            var simpleInjectorContainer = new Container();

            var container = new SimpleInjectorDiContainer(simpleInjectorContainer);

            new DiContainerTestHelper().TestGetInstanceThrowsException<ITestService>(container);
        }

        [Fact]
        public void GetInstancesTest()
        {
            var _ = new[] { typeof(TestServiceWithNoCtorParam).Assembly};
            var simpleInjectorContainer = new Container();
            simpleInjectorContainer.Collection.Register<ITestService>(typeof(TestService2), typeof(TestServiceWithNoCtorParam));
            
            simpleInjectorContainer.Verify();
            var container = new SimpleInjectorDiContainer(simpleInjectorContainer);

            new DiContainerTestHelper().TestGetInstances<ITestService>(container, typeof(TestService2), typeof(TestServiceWithNoCtorParam));
        }

        [Fact]
        public void GetInstances_NoServicesFound_ReturnsEmptyArray()
        {
            var container = new SimpleInjectorDiContainer(new Container());

            new DiContainerTestHelper().GetInstances_NoServicesFound_ReturnsEmptyArray<ITestService>(container);
        }

        [Fact]
        public void TryGetInstance_NoServiceFound_ReturnsFalseAndNull()
        {
            var container = new SimpleInjectorDiContainer(new Container());

            new DiContainerTestHelper().TryGetInstance_NoServiceFound_ReturnsFalseAndNull<ITestService>(container);
        }

        [Fact]
        public void TryGetInstance_ServiceFound_ReturnsTrueAndInstance()
        {
            var simpleInjectorContainer = new Container();
            simpleInjectorContainer.Register<ITestService, TestService2>();
            var container = new SimpleInjectorDiContainer(simpleInjectorContainer);

            new DiContainerTestHelper().TryGetInstance_ServiceFound_ReturnsTrueAndInstance<ITestService>(container);
        }

        [Fact]
        public void CreateScopeTest()
        {
            var simpleInjectorContainer = new Container();
            simpleInjectorContainer.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            simpleInjectorContainer.Register<ITestService, TestServiceWithNoCtorParam>(Lifestyle.Scoped);

            var container = new SimpleInjectorDiContainer(simpleInjectorContainer);

            new DiContainerTestHelper().TestCreateScope<ITestService, TestServiceWithNoCtorParam>(container);

            new DiContainerTestHelper().TestScopeDisposeDouble(container);
        }
    }
}
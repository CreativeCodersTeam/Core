using System;
using CreativeCoders.Core.UnitTests.Di.Helper;
using CreativeCoders.Di.MsServiceProvider;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Di.MsServiceProvider
{
    public class ServiceProviderContainerBuilderTests
    {
        [Fact]
        public void CtorTestAsserts()
        {
            Assert.Throws<ArgumentNullException>(() => new ServiceProviderDiContainerBuilder(null));

            var _ = new ServiceProviderDiContainerBuilder(new ServiceCollection());
        }

        [Fact]
        public void AddTransientTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddTransient(() =>
                new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void AddSingletonTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddSingleton(() =>
                new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void AddScopedTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddScoped(() =>
                new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void AddTransientCollectionTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddTransientCollection(() =>
                new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void AddSingletonCollectionTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddSingletonCollection(() =>
                new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void AddScopedCollectionTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddScopedCollection(() =>
                new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void AddTransientNamedTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddTransientNamed(() =>
                new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void AddScopedNamedTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddScopedNamed(() =>
                new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void AddSingletonNamedTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddSingletonNamed(() =>
                new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void AddTransientNamedTestWithOutRegistration()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddTransientNamedTestNoRegistration(() =>
                new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void GetInstance_GetContainerScoped_ReturnsScopedContainer()
        {
            new DiContainerBuilderTestHelper().GetInstance_GetDiContainerInScope_GetsScopedContainer(
                () => new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void GetInstances_CollectionAndNamedRegistrations_ReturnsAllInstances()
        {
            new DiContainerBuilderTestHelper()
                .GetInstances_CollectionAndNamedRegistrations_ReturnsAllInstances(
                    () => new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void ClassFactory_GetFactoryForInterface_ReturnsInstance()
        {
            new DiContainerBuilderTestHelper()
                .ClassFactory_GetFactoryForInterface_ReturnsInstance(
                    () =>
                        new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void RegisterImplementations_ForAllAssemblies_ReturnsCorrectInstances()
        {
            new DiContainerBuilderTestHelper()
                .RegisterImplementations_ForAllAssemblies_ReturnsCorrectInstances(
                    () =>
                        new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void RegisterImplementations_ForAssemblies_ReturnsCorrectInstances()
        {
            new DiContainerBuilderTestHelper()
                .RegisterImplementations_ForAssemblies_ReturnsCorrectInstances(
                    () =>
                        new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void RegisterImplementations_ForAssembly_ReturnsCorrectInstance()
        {
            new DiContainerBuilderTestHelper()
                .RegisterImplementations_ForAssembly_ReturnsCorrectInstance(
                    () =>
                        new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }

        [Fact]
        public void RegisterImplementations_ForType_ReturnsCorrectInstance()
        {
            new DiContainerBuilderTestHelper()
                .RegisterImplementations_ForType_ReturnsCorrectInstance(
                    () =>
                        new ServiceProviderDiContainerBuilder(new ServiceCollection()));
        }
    }
}
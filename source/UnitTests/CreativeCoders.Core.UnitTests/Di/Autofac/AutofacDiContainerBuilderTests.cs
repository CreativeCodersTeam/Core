using System;
using Autofac;
using CreativeCoders.Core.UnitTests.Di.Helper;
using CreativeCoders.Di.Autofac;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Di.Autofac
{
    public class AutofacDiContainerBuilderTests
    {
        [Fact]
        public void CtorTestAsserts()
        {
            Assert.Throws<ArgumentNullException>(() => new AutofacDiContainerBuilder(null));

            var _ = new AutofacDiContainerBuilder(new ContainerBuilder());
        }

        [Fact]
        public void AddTransientTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddTransient(() =>
                new AutofacDiContainerBuilder(CreateContainer()));

            tests.TestAddTransient(() =>
                new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void AddSingletonTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddSingleton(() =>
                new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void AddScopedTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddScoped(() =>
                new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void AddTransientCollectionTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddTransientCollection(() =>
                new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void AddSingletonCollectionTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddSingletonCollection(() =>
                new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void AddScopedCollectionTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddScopedCollection(() =>
                new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void AddTransientNamedTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddTransientNamed(() =>
                new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void AddScopedNamedTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddScopedNamed(() =>
                new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void AddSingletonNamedTest()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddSingletonNamed(() =>
                new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void AddTransientNamedTestWithOutRegistration()
        {
            var tests = new DiContainerBuilderTestHelper();

            tests.TestAddTransientNamedTestNoRegistration(() =>
                new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void GetInstance_GetContainerScoped_ReturnsScopedContainer()
        {
            var autofacContainerBuilder = new ContainerBuilder();

            new DiContainerBuilderTestHelper().GetInstance_GetDiContainerInScope_GetsScopedContainer(
                () => new AutofacDiContainerBuilder(autofacContainerBuilder));
        }

        [Fact]
        public void GetInstances_CollectionAndNamedRegistrations_ReturnsAllInstances()
        {
            new DiContainerBuilderTestHelper()
                .GetInstances_CollectionAndNamedRegistrations_ReturnsAllInstances(
                    () =>
                        new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void ClassFactory_GetFactoryForInterface_ReturnsInstance()
        {
            new DiContainerBuilderTestHelper()
                .ClassFactory_GetFactoryForInterface_ReturnsInstance(
                    () =>
                        new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void AddSingleton_OpenGeneric_GetInstanceReturnsSameInstance()
        {
            new DiContainerBuilderTestHelper()
                .AddSingleton_OpenGeneric_GetInstanceReturnsSameInstance(
                    () =>
                        new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void AddTransient_OpenGeneric_GetInstanceReturnsDifferentInstances()
        {
            new DiContainerBuilderTestHelper()
                .AddTransient_OpenGeneric_GetInstanceReturnsDifferentInstances(
                    () =>
                        new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void RegisterImplementations_ForAllAssemblies_ReturnsCorrectInstances()
        {
            new DiContainerBuilderTestHelper()
                .RegisterImplementations_ForAllAssemblies_ReturnsCorrectInstances(
                    () =>
                        new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void RegisterImplementations_ForAssemblies_ReturnsCorrectInstances()
        {
            new DiContainerBuilderTestHelper()
                .RegisterImplementations_ForAssemblies_ReturnsCorrectInstances(
                    () =>
                        new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void RegisterImplementations_ForAssembly_ReturnsCorrectInstance()
        {
            new DiContainerBuilderTestHelper()
                .RegisterImplementations_ForAssembly_ReturnsCorrectInstance(
                    () =>
                        new AutofacDiContainerBuilder(CreateContainer()));
        }

        [Fact]
        public void RegisterImplementations_ForType_ReturnsCorrectInstance()
        {
            new DiContainerBuilderTestHelper()
                .RegisterImplementations_ForType_ReturnsCorrectInstance(
                    () =>
                        new AutofacDiContainerBuilder(CreateContainer()));
        }

        private static ContainerBuilder CreateContainer()
        {
            var container = new ContainerBuilder();
            return container;
        }
    }
}
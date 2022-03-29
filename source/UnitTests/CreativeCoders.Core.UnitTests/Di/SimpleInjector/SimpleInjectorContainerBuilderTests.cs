using System;
using CreativeCoders.Core.UnitTests.Di.Helper;
using CreativeCoders.Di.SimpleInjector;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Di.SimpleInjector;

public class SimpleInjectorContainerBuilderTests
{
    [Fact]
    public void CtorTestAsserts()
    {
        Assert.Throws<ArgumentNullException>(() => new SimpleInjectorDiContainerBuilder(null, null));

        Assert.Throws<ArgumentNullException>(() =>
            new SimpleInjectorDiContainerBuilder(new Container(), null));

        Assert.Throws<ArgumentNullException>(() =>
            new SimpleInjectorDiContainerBuilder(null));

        var _ = new SimpleInjectorDiContainerBuilder(new Container());
    }

    [Fact]
    public void AddTransientTest()
    {
        var tests = new DiContainerBuilderTestHelper();

        tests.TestAddTransient(() =>
            new SimpleInjectorDiContainerBuilder(CreateContainer(false)));

        tests.TestAddTransient(() =>
            new SimpleInjectorDiContainerBuilder(CreateContainer(false)).WithVerify());
    }

    [Fact]
    public void AddSingletonTest()
    {
        var tests = new DiContainerBuilderTestHelper();

        tests.TestAddSingleton(() =>
            new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    [Fact]
    public void AddScopedTest()
    {
        var tests = new DiContainerBuilderTestHelper();

        tests.TestAddScoped(() =>
            new SimpleInjectorDiContainerBuilder(CreateContainer(true)));
    }

    [Fact]
    public void AddTransientCollectionTest()
    {
        var tests = new DiContainerBuilderTestHelper();

        tests.TestAddTransientCollection(() =>
            new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    [Fact]
    public void AddSingletonCollectionTest()
    {
        var tests = new DiContainerBuilderTestHelper();

        tests.TestAddSingletonCollection(() =>
            new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    [Fact]
    public void AddScopedCollectionTest()
    {
        var tests = new DiContainerBuilderTestHelper();

        tests.TestAddScopedCollection(() =>
            new SimpleInjectorDiContainerBuilder(CreateContainer(true)));
    }

    [Fact]
    public void AddTransientNamedTest()
    {
        var tests = new DiContainerBuilderTestHelper();

        tests.TestAddTransientNamed(() =>
            new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    [Fact]
    public void AddScopedNamedTest()
    {
        var tests = new DiContainerBuilderTestHelper();

        tests.TestAddScopedNamed(() =>
            new SimpleInjectorDiContainerBuilder(CreateContainer(true)));
    }

    [Fact]
    public void AddSingletonNamedTest()
    {
        var tests = new DiContainerBuilderTestHelper();

        tests.TestAddSingletonNamed(() =>
            new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    [Fact]
    public void AddTransientNamedTestWithOutRegistration()
    {
        var tests = new DiContainerBuilderTestHelper();

        tests.TestAddTransientNamedTestNoRegistration(() =>
            new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    [Fact]
    public void GetInstance_GetContainerScoped_ReturnsScopedContainer()
    {
        var simpleInjectorContainer = new Container();
            
        new DiContainerBuilderTestHelper().GetInstance_GetDiContainerInScope_GetsScopedContainer(
            () => new SimpleInjectorDiContainerBuilder(simpleInjectorContainer));
    }

    [Fact]
    public void GetInstances_CollectionAndNamedRegistrations_ReturnsAllInstances()
    {
        new DiContainerBuilderTestHelper()
            .GetInstances_CollectionAndNamedRegistrations_ReturnsAllInstances(
                () =>
                    new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    [Fact]
    public void ClassFactory_GetFactoryForInterface_ReturnsInstance()
    {
        new DiContainerBuilderTestHelper()
            .ClassFactory_GetFactoryForInterface_ReturnsInstance(
                () =>
                    new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    [Fact]
    public void RegisterImplementations_ForAllAssemblies_ReturnsCorrectInstances()
    {
        new DiContainerBuilderTestHelper()
            .RegisterImplementations_ForAllAssemblies_ReturnsCorrectInstances(
                () =>
                    new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    [Fact]
    public void RegisterImplementations_ForAssemblies_ReturnsCorrectInstances()
    {
        new DiContainerBuilderTestHelper()
            .RegisterImplementations_ForAssemblies_ReturnsCorrectInstances(
                () =>
                    new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    [Fact]
    public void RegisterImplementations_ForAssembly_ReturnsCorrectInstance()
    {
        new DiContainerBuilderTestHelper()
            .RegisterImplementations_ForAssembly_ReturnsCorrectInstance(
                () =>
                    new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    [Fact]
    public void RegisterImplementations_ForType_ReturnsCorrectInstance()
    {
        new DiContainerBuilderTestHelper()
            .RegisterImplementations_ForType_ReturnsCorrectInstance(
                () =>
                    new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }
        
    [Fact]
    public void AddTransientCollectionFor_Type_ReturnsCorrectInstances()
    {
        new DiContainerBuilderTestHelper()
            .AddTransientCollectionFor_Type_ReturnsCorrectInstances(
                () => 
                    new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }
        
    [Fact]
    public void AddScopedCollectionFor_Type_ReturnsCorrectInstances()
    {
        new DiContainerBuilderTestHelper()
            .AddScopedCollectionFor_Type_ReturnsCorrectInstances(
                () => 
                    new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }
        
    [Fact]
    public void AddSingletonCollectionFor_Type_ReturnsCorrectInstances()
    {
        new DiContainerBuilderTestHelper()
            .AddSingletonCollectionFor_Type_ReturnsCorrectInstances(
                () => 
                    new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }
        
    [Fact]
    public void AddTransientCollectionFor_ForNoneInterfaceType_ThrowsException()
    {
        new DiContainerBuilderTestHelper()
            .AddTransientCollectionFor_ForNoneInterfaceType_ThrowsException(
                () =>
                    new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    [Fact]
    public void AddTransientCollectionFor_ReflectionType_ReturnsCorrectInstances()
    {
        new DiContainerBuilderTestHelper()
            .AddTransientCollectionFor_ReflectionType_ReturnsCorrectInstances(
                () =>
                    new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    [Fact]
    public void AddScopedCollectionFor_ReflectionType_ReturnsCorrectInstances()
    {
        new DiContainerBuilderTestHelper()
            .AddScopedCollectionFor_ReflectionType_ReturnsCorrectInstances(
                () =>
                    new SimpleInjectorDiContainerBuilder(CreateContainer(true)));
    }

    [Fact]
    public void AddSingletonCollectionFor_ReflectionType_ReturnsCorrectInstances()
    {
        new DiContainerBuilderTestHelper()
            .AddSingletonCollectionFor_ReflectionType_ReturnsCorrectInstances(
                () =>
                    new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    [Fact]
    public void AddTransientCollectionFor_ReflectionForNoneInterfaceType_ThrowsException()
    {
        new DiContainerBuilderTestHelper()
            .AddTransientCollectionFor_ReflectionForNoneInterfaceType_ThrowsException(
                () =>
                    new SimpleInjectorDiContainerBuilder(CreateContainer(false)));
    }

    private static Container CreateContainer(bool createScoped)
    {
        var container = new Container();
        if (!createScoped)
        {
            return container;
        }
            
        container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
        return AsyncScopedLifestyle.BeginScope(container).Container;
    }
}
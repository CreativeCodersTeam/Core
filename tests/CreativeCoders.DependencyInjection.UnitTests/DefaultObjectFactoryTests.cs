using CreativeCoders.DependencyInjection.UnitTests.TestData;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.DependencyInjection.UnitTests;

public class DefaultObjectFactoryTests
{
    [Fact]
    public void GetInstance_ClassWithOutServiceRegistration_EachTimeCalledInstanceIsCreated()
    {
        var services = new ServiceCollection();

        services.AddObjectFactory();

        var sp = services.BuildServiceProvider();

        var factory = sp.GetRequiredService<IObjectFactory<TestClassWithOutCtorParams>>();

        // Act
        var instance0 = factory.GetInstance();

        var instance1 = factory.GetInstance();

        // Assert
        instance0
            .Should()
            .NotBeNull();

        instance1
            .Should()
            .NotBeNull();

        instance0
            .Should()
            .NotBeSameAs(instance1);
    }

    [Fact]
    public void CreateInstance_ClassWithOutServiceRegistration_EachTimeCalledInstanceIsCreated()
    {
        var services = new ServiceCollection();

        services.AddObjectFactory();

        var sp = services.BuildServiceProvider();

        var factory = sp.GetRequiredService<IObjectFactory<TestClassWithOutCtorParams>>();

        // Act
        var instance0 = factory.CreateInstance();

        var instance1 = factory.CreateInstance();

        // Assert
        instance0
            .Should()
            .NotBeNull();

        instance1
            .Should()
            .NotBeNull();

        instance0
            .Should()
            .NotBeSameAs(instance1);
    }

    [Fact]
    public void CreateInstance_ClassWithStringParam_CtorIsCalledWithParam()
    {
        const string expectedText = "HelloWorld";

        var services = new ServiceCollection();

        services.AddObjectFactory();

        var sp = services.BuildServiceProvider();

        var factory = sp.GetRequiredService<IObjectFactory<TestClassWithStringParam>>();

        // Act
        var instance = factory.CreateInstance(expectedText);

        // Assert
        instance
            .Should()
            .NotBeNull();

        instance.Text
            .Should()
            .Be(expectedText);
    }

    [Fact]
    public void GetInstance_ClassWithTestServiceParam_InstanceCreatedWithDependencyResolved()
    {
        var services = new ServiceCollection();

        var testService = A.Fake<ITestService>();

        services.AddSingleton(testService);

        services.AddObjectFactory();

        var sp = services.BuildServiceProvider();

        var factory = sp.GetRequiredService<IObjectFactory<TestClassWithTestServiceParam>>();

        // Act
        var instance = factory.GetInstance();

        // Assert
        instance
            .Should()
            .NotBeNull();

        instance.TestService
            .Should()
            .BeSameAs(testService);
    }

    [Fact]
    public void GetInstance_ClassWithTestServiceParamRegistered_InstanceCreatedWithDependencyResolved()
    {
        var services = new ServiceCollection();

        var testService = A.Fake<ITestService>();

        services.AddSingleton(testService);

        services.AddObjectFactory();

        services.AddSingleton<TestClassWithTestServiceParam>();

        var sp = services.BuildServiceProvider();

        var factory = sp.GetRequiredService<IObjectFactory<TestClassWithTestServiceParam>>();

        // Act
        var instance0 = factory.GetInstance();

        var instance1 = factory.GetInstance();

        // Assert
        instance0
            .Should()
            .NotBeNull();

        instance0.TestService
            .Should()
            .BeSameAs(testService);

        instance1
            .Should()
            .NotBeNull();

        instance1
            .Should()
            .BeSameAs(instance0);
    }

    [Fact]
    public void CreateInstance_ClassWithTestServiceParam_DependencyIsResolved()
    {
        var services = new ServiceCollection();

        var testService = A.Fake<ITestService>();

        services.AddSingleton(testService);

        services.AddObjectFactory();

        var sp = services.BuildServiceProvider();

        var factory = sp.GetRequiredService<IObjectFactory<TestClassWithTestServiceParam>>();

        // Act
        var instance = factory.CreateInstance();

        // Assert
        instance
            .Should()
            .NotBeNull();

        instance.TestService
            .Should()
            .BeSameAs(testService);
    }

    [Fact]
    public void CreateInstance_ClassWithMixedCtorParams_EachTimeCalledInstanceIsCreated()
    {
        const string expectedText = "A1234";

        var services = new ServiceCollection();

        var testService = A.Fake<ITestService>();

        services.AddSingleton(testService);

        services.AddObjectFactory();

        var sp = services.BuildServiceProvider();

        var factory = sp.GetRequiredService<IObjectFactory<TestClassWithMixedCtorParams>>();

        // Act
        var instance = factory.CreateInstance(expectedText);

        // Assert
        instance
            .Should()
            .NotBeNull();

        instance.Text
            .Should()
            .Be(expectedText);

        instance.TestService
            .Should()
            .BeSameAs(testService);
    }
}

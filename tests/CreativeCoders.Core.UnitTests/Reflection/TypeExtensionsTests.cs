using System;
using CreativeCoders.Core.Reflection;
using CreativeCoders.Core.UnitTests.Reflection.TestData;
using FakeItEasy;
using AwesomeAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

#nullable enable
namespace CreativeCoders.Core.UnitTests.Reflection;

public class TypeExtensionsTests
{
    [Fact]
    public void CreateGenericInstance_WithoutCtorParameters_ReturnInstanceWithCorrectTypeAndData()
    {
        var instance = typeof(GenericClass<>).CreateGenericInstance(typeof(int));

        Assert.NotNull(instance);
        Assert.IsType<GenericClass<int>>(instance);
        Assert.Equal(0, ((GenericClass<int>) instance).Data);
    }

    [Fact]
    public void CreateGenericInstance_WithoutCtorParameters_ReturnInstanceWithCorrectType()
    {
        var instance = typeof(GenericClass<>).CreateGenericInstance(typeof(int), 1234);

        Assert.NotNull(instance);
        Assert.IsType<GenericClass<int>>(instance);
        Assert.Equal(1234, ((GenericClass<int>) instance).Data);
    }

    [Fact]
    public void GetDefault_ForInt_ReturnsZero()
    {
        var value = typeof(int).GetDefault();

        value
            .Should()
            .BeOfType<int>()
            .And
            .Be(0);
    }

    [Fact]
    public void GetDefault_ForBool_ReturnsFalse()
    {
        var value = typeof(bool).GetDefault();

        value
            .Should()
            .BeOfType<bool>()
            .And
            .Be(false);
    }

    [Fact]
    public void GetDefault_ForString_ReturnsNull()
    {
        var value = typeof(string).GetDefault();

        Assert.Null(value);
    }

    [Fact]
    public void CreateInstance_ForObject_ReturnsNewObject()
    {
        var sp = A.Fake<IServiceProvider>();

        // Act
        var instance = typeof(object).CreateInstance<object>(sp);

        // Assert
        instance
            .Should()
            .BeOfType<object>();
    }


    [Fact]
    public void CreateInstance_ForSimpleClassWithStringArg_ReturnsSimpleClassWithText()
    {
        const string expectedText = "TestText";

        var services = new ServiceCollection();

        services.AddTransient(_ => expectedText);

        var sp = services.BuildServiceProvider();

        // Act
        var instance = typeof(TestSimpleClass).CreateInstance<TestSimpleClass>(sp);

        // Assert
        instance
            .Should()
            .NotBeNull();

        instance!.Text
            .Should()
            .Be(expectedText);
    }

    [Fact]
    public void CreateInstance_ForSimpleClassWithOptionsArg_ReturnsSimpleClassOptions()
    {
        const string expectedText = "TestText";

        var services = new ServiceCollection();

        var options = new TestSimpleClassOptions {Value = expectedText};

        services.AddTransient<ITestSimpleClassOptions>(_ => options);

        var sp = services.BuildServiceProvider();

        // Act
        var instance =
            typeof(TestSimpleClassWithOptionsArg).CreateInstance<TestSimpleClassWithOptionsArg>(sp);

        // Assert
        instance
            .Should()
            .NotBeNull();

        instance!.Options
            .Should()
            .BeSameAs(options);
    }

    [Fact]
    public void CreateInstance_ForSimpleClassWithOptionsArgAsObjectArgument_ReturnsSimpleClassOptions()
    {
        const string expectedText = "TestText";

        var services = new ServiceCollection();

        var options = new TestSimpleClassOptions {Value = expectedText};

        var sp = services.BuildServiceProvider();

        // Act
        var instance =
            typeof(TestSimpleClassWithOptionsArg).CreateInstance<TestSimpleClassWithOptionsArg>(sp, options);

        // Assert
        instance
            .Should()
            .NotBeNull();

        instance!.Options
            .Should()
            .BeSameAs(options);
    }

    [Fact]
    public void CreateInstance_ForSimpleClassWithOptionsArgAsArgument_ReturnsSimpleClassOptions()
    {
        const string expectedText = "TestText";

        var services = new ServiceCollection();

        var options = new TestSimpleClassOptions {Value = expectedText};

        var sp = services.BuildServiceProvider();

        // Act
        var instance =
            typeof(TestSimpleClassWithOptionsArgAsClass).CreateInstance<TestSimpleClassWithOptionsArgAsClass>(
                sp,
                options);

        // Assert
        instance
            .Should()
            .NotBeNull();

        instance!.Options
            .Should()
            .BeSameAs(options);
    }

    [Fact]
    public void CreateInstance_ForSimpleClassWithOptionsArgTooMuchArgs_ReturnsNull()
    {
        const string expectedText = "TestText";

        var services = new ServiceCollection();

        var options = new TestSimpleClassOptions {Value = expectedText};

        var sp = services.BuildServiceProvider();

        // Act
        var instance =
            typeof(TestSimpleClassWithOptionsArgAsClass).CreateInstance<TestSimpleClassWithOptionsArgAsClass>(
                sp,
                options, "Test");

        // Assert
        instance
            .Should()
            .BeNull();
    }

    [Fact]
    public void CreateInstance_ForSimpleClassWithOptionsArgAsArgumentAndService_ReturnsSimpleClassOptions()
    {
        const string expectedText = "TestText";

        var services = new ServiceCollection();

        var options = new TestSimpleClassOptions {Value = expectedText};

        var service = new TestService();

        services.AddSingleton<ITestService>(_ => service);

        var sp = services.BuildServiceProvider();

        // Act
        var instance =
            typeof(TestSimpleClassWithOptionsArgAndService)
                .CreateInstance<TestSimpleClassWithOptionsArgAndService>(sp,
                    options);

        // Assert
        instance
            .Should()
            .NotBeNull();

        instance!.Options
            .Should()
            .BeSameAs(options);

        instance.TestService
            .Should()
            .BeSameAs(service);
    }
}

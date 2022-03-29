using System;
using JetBrains.Annotations;
using Xunit;

namespace CreativeCoders.Core.UnitTests;

public class ActivatorClassFactoryTests
{
    [Fact]
    public void Create_WithClassTypeAsParameter_ReturnsCorrectClass()
    {
        var classFactory = new ActivatorClassFactory();

        var instance = classFactory.Create(typeof(ActivatorTestClass));

        Assert.IsType<ActivatorTestClass>(instance);
    }

    [Fact]
    public void Create_WithClassTypeAsParameterWithCtorParameters_ThrowsException()
    {
        var classFactory = new ActivatorClassFactory();

        Assert.Throws<MissingMethodException>(() =>
            classFactory.Create(typeof(ActivatorTestClassWithCtorParameters)));
    }

    [Fact]
    public void Create_WithSetupWithClassTypeAsParameter_ReturnsCorrectClass()
    {
        var classFactory = new ActivatorClassFactory();

        var instance = classFactory.Create(typeof(ActivatorTestClass),
            x => ((ActivatorTestClass) x).Text = "1234");

        Assert.IsType<ActivatorTestClass>(instance);
        Assert.Equal("1234", ((ActivatorTestClass) instance).Text);
    }

    [Fact]
    public void Create_WithClassTypeAsGenericParameter_ReturnsCorrectClass()
    {
        var classFactory = new ActivatorClassFactory();

        var instance = classFactory.Create<ActivatorTestClass>();

        Assert.IsType<ActivatorTestClass>(instance);
    }

    [Fact]
    public void Create_WithSetupWithClassTypeAsGenericParameter_ReturnsCorrectClass()
    {
        var classFactory = new ActivatorClassFactory();

        var instance = classFactory.Create<ActivatorTestClass>(x => x.Text = "abcd");

        Assert.IsType<ActivatorTestClass>(instance);
        Assert.Equal("abcd", instance.Text);
    }

    [Fact]
    public void Create_GenericWithClassTypeAsGenericParameter_ReturnsCorrectClass()
    {
        var classFactory = new ActivatorClassFactory<ActivatorTestClass>();

        var instance = classFactory.Create();

        Assert.IsType<ActivatorTestClass>(instance);
    }

    [Fact]
    public void Create_GenericWithSetupWithClassTypeAsGenericParameter_ReturnsCorrectClass()
    {
        var classFactory = new ActivatorClassFactory<ActivatorTestClass>();

        var instance = classFactory.Create(x => x.Text = "abcd");

        Assert.IsType<ActivatorTestClass>(instance);
        Assert.Equal("abcd", instance.Text);
    }

    [Fact]
    public void Create_GenericWithClassTypeAsGenericParameterForWrongType_ThrowsException()
    {
        var classFactory = new ActivatorClassFactory<ActivatorTestClassWithCtorParameters>();

        Assert.Throws<MissingMethodException>(() => classFactory.Create());
    }

    [Fact]
    public void Create_GenericWithSetupWithClassTypeAsGenericParameterForWrongType_ThrowsException()
    {
        var classFactory = new ActivatorClassFactory<ActivatorTestClassWithCtorParameters>();

        Assert.Throws<MissingMethodException>(() => classFactory.Create(x => x.Text = "abcd"));
    }
}

public class ActivatorTestClass
{
    public string Text { get; set; }
}

[UsedImplicitly]
public class ActivatorTestClassWithCtorParameters
{
    public ActivatorTestClassWithCtorParameters(string text)
    {
        Text = text;
    }

    public string Text { get; set; }
}

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using CreativeCoders.Core.Executing;
using CreativeCoders.Core.Reflection;
using FakeItEasy;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

#nullable enable

namespace CreativeCoders.Core.UnitTests.Reflection;

public class MethodInfoExtensionsTests
{
    [Fact]
    public void ParametersAreEqual_SingleParameterNotEqual_ReturnsFalse()
    {
        var interfaceType = typeof(ITestInterfaceWithMethods);

        var test1Method = interfaceType.GetMethod(nameof(ITestInterfaceWithMethods.Test1))!;
        var test2Method = interfaceType.GetMethod(nameof(ITestInterfaceWithMethods.Test2));

        // Act
        var areEqual = test1Method.ParametersAreEqual(test2Method!);

        // Assert
        areEqual
            .Should()
            .BeFalse();
    }

    [Fact]
    public void ParametersAreEqual_SingleParameterEqual_ReturnsTrue()
    {
        var interfaceType = typeof(ITestInterfaceWithMethods);

        var test1Method = interfaceType.GetMethod(nameof(ITestInterfaceWithMethods.Test1))!;
        var test3Method = interfaceType.GetMethod(nameof(ITestInterfaceWithMethods.Test3));

        // Act
        var areEqual = test1Method.ParametersAreEqual(test3Method!);

        // Assert
        areEqual
            .Should()
            .BeTrue();
    }

    [Fact]
    public void ParametersAreEqual_MultipleParametersNotEqual_ReturnsFalse()
    {
        var interfaceType = typeof(ITestInterfaceWithMethods);

        var test4Method = interfaceType.GetMethod(nameof(ITestInterfaceWithMethods.Test4));
        var test5Method = interfaceType.GetMethod(nameof(ITestInterfaceWithMethods.Test5))!;

        Assert.False(test4Method!.ParametersAreEqual(test5Method));
    }

    [Fact]
    public void ParametersAreEqual_MultipleParametersEqual_ReturnsTrue()
    {
        var interfaceType = typeof(ITestInterfaceWithMethods);

        var test4Method = interfaceType.GetMethod(nameof(ITestInterfaceWithMethods.Test4))!;
        var test6Method = interfaceType.GetMethod(nameof(ITestInterfaceWithMethods.Test6))!;

        Assert.True(test4Method.ParametersAreEqual(test6Method));
    }

    [Fact]
    public void MatchesMethod_ExactSameSignature_ReturnsTrue()
    {
        var methodInfo = typeof(ITestInterfaceWithMethods)
            .GetMethod(nameof(ITestInterfaceWithMethods.TestGeneric1))?
            .MakeGenericMethod(typeof(int))!;

        Assert.True(methodInfo.MatchesMethod(
            typeof(ITestInterfaceWithMethods).GetMethod(nameof(ITestInterfaceWithMethods.TestGeneric1))!));
    }

    [Fact]
    public void MatchesMethod_SameSignatureButDifferentGenericParameters_ReturnsFalse()
    {
        var methodInfoInt = typeof(ITestInterfaceWithMethods)
            .GetMethod(nameof(ITestInterfaceWithMethods.TestGeneric1))?
            .MakeGenericMethod(typeof(int));

        var methodInfoDouble = typeof(ITestInterfaceWithMethods)
            .GetMethod(nameof(ITestInterfaceWithMethods.TestGeneric1))?
            .MakeGenericMethod(typeof(double));

        Assert.False(methodInfoInt!.MatchesMethod(methodInfoDouble!));
    }

    [Fact]
    public void Execute_VoidOneArgFromServiceProviderOneFromArgs_MethodIsCalledCorrect()
    {
        const string text = "TestText";

        var executable = A.Fake<IExecutable<string>>();

        var testExecute = new TestExecute();

        var voidMethod = testExecute.GetType().GetMethod(nameof(TestExecute.VoidMethod));

        var services = new ServiceCollection();

        services.AddSingleton(executable);

        // Act
        voidMethod!.Execute(testExecute, services.BuildServiceProvider(), text);

        // Arrange
        A.CallTo(() => executable.Execute(text)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Execute_ResultWithOneArgFromServiceProviderOneFromArgs_MethodIsCalledCorrectAndReturnsResult()
    {
        const string text = "TestText";

        var executable = new TestExecutable();

        var testExecute = new TestExecute();

        var textMethod = testExecute.GetType().GetMethod(nameof(TestExecute.TextMethod));

        var services = new ServiceCollection();

        services.AddSingleton(executable);

        // Act
        var result = textMethod!.Execute<string>(testExecute, services.BuildServiceProvider(), text);

        // Arrange
        result
            .Should()
            .Be($"{text}{text}");
    }
}

[SuppressMessage("Performance", "CA1822")]
[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
public class TestExecutable
{
    public string DoSomeThing(string text)
    {
        return $"{text}{text}";
    }
}

[SuppressMessage("Performance", "CA1822")]
public class TestExecute
{
    public void VoidMethod(IExecutable<string> executable, string text)
    {
        executable.Execute(text);
    }

    public string TextMethod(TestExecutable executable, string text)
    {
        return executable.DoSomeThing(text);
    }
}

[PublicAPI]
public interface ITestInterfaceWithMethods
{
    void Test1(int i);

    void Test2(long i);

    void Test3(int i);

    void Test4(string text, bool b, StringBuilder sb);

    void Test5(string text, object b, StringBuilder sb);

    void Test6(string text, bool b, StringBuilder sb);

    MethodInfo TestGeneric1<T>(int i);
}

[UsedImplicitly]
public class TestInterfaceWithMethods : ITestInterfaceWithMethods
{
    public void Test1(int i)
    {
        throw new System.NotImplementedException();
    }

    public void Test2(long i)
    {
        throw new System.NotImplementedException();
    }

    public void Test3(int i)
    {
        throw new System.NotImplementedException();
    }

    public void Test4(string text, bool b, StringBuilder sb)
    {
        throw new System.NotImplementedException();
    }

    public void Test5(string text, object b, StringBuilder sb)
    {
        throw new System.NotImplementedException();
    }

    public void Test6(string text, bool b, StringBuilder sb)
    {
        throw new System.NotImplementedException();
    }

    public MethodInfo TestGeneric1<T>(int i)
    {
        var currentMethod = MethodBase.GetCurrentMethod();
        return (currentMethod as MethodInfo)!;
    }
}

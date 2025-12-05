using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AwesomeAssertions;
using CreativeCoders.ProcessUtils;
using CreativeCoders.ProcessUtils.Execution;
using FakeItEasy;
using JetBrains.Annotations;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ProcessUtils.Execution;

#nullable enable

[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
[SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
public class ProcessExecutorExtensionsTests
{
    [PublicAPI]
    private sealed class TestVars
    {
        // Simple POCO used to verify that the extensions convert an object to a dictionary via ToDictionary()
        public string? Name { get; init; }

        public int Number { get; init; }
    }

    [Fact]
    public void Generic_Execute_WithObjectVars_ForwardsDictionary_AndReturnsValue()
    {
        // Arrange
        var vars = new TestVars { Name = "Alice", Number = 7 };

        var fakeExecutor = A.Fake<IProcessExecutor<string>>();

        IDictionary<string, object?>? capturedVars = null;
        A.CallTo(() => fakeExecutor.Execute(A<IDictionary<string, object?>>._))
            .Invokes(call => capturedVars = call.GetArgument<IDictionary<string, object?>>(0))
            .Returns("ok");

        // Act
        var result = ProcessExecutorGenericExtensions.Execute(fakeExecutor, vars);

        // Assert
        result
            .Should().Be("ok");

        capturedVars
            .Should().NotBeNull()
            .And.HaveCount(2)
            .And.BeEquivalentTo(new Dictionary<string, object?>()
            {
                { "Name", "Alice" },
                { "Number", 7 }
            });
    }

    [Fact]
    public async Task Generic_ExecuteAsync_WithObjectVars_ForwardsDictionary_AndReturnsValue()
    {
        // Arrange
        var vars = new TestVars { Name = "Bob", Number = 42 };

        var fakeExecutor = A.Fake<IProcessExecutor<string>>();

        IDictionary<string, object?>? capturedVars = null;
        A.CallTo(() => fakeExecutor.ExecuteAsync(A<IDictionary<string, object?>>._))
            .Invokes(call => capturedVars = call.GetArgument<IDictionary<string, object?>>(0))
            .Returns(Task.FromResult<string?>("123"));

        // Act
        var result = await ProcessExecutorGenericExtensions.ExecuteAsync(fakeExecutor, vars);

        // Assert
        result
            .Should().Be("123");

        capturedVars
            .Should().NotBeNull()
            .And.HaveCount(2)
            .And.BeEquivalentTo(new Dictionary<string, object?>()
            {
                { "Name", "Bob" },
                { "Number", 42 }
            });
    }

    [Fact]
    public void Generic_ExecuteEx_WithObjectVars_ForwardsDictionary_AndReturnsResult()
    {
        // Arrange
        var vars = new TestVars { Name = "X", Number = 1 };

        var fakeExecutor = A.Fake<IProcessExecutor<string>>();
        var fakeProcess = A.Fake<IProcess>();
        var expected = new ProcessExecutionResult<string?>(fakeProcess, "val");

        IDictionary<string, object?>? capturedVars = null;
        A.CallTo(() => fakeExecutor.ExecuteEx(A<IDictionary<string, object?>>._))
            .Invokes(call => capturedVars = call.GetArgument<IDictionary<string, object?>>(0))
            .Returns(expected);

        // Act
        var result = ProcessExecutorGenericExtensions.ExecuteEx(fakeExecutor, vars);

        // Assert
        result
            .Should().BeSameAs(expected);

        result.Value
            .Should().Be("val");

        result.Process
            .Should().BeSameAs(fakeProcess);

        capturedVars
            .Should().NotBeNull()
            .And.HaveCount(2)
            .And.BeEquivalentTo(new Dictionary<string, object?>()
            {
                { "Name", "X" },
                { "Number", 1 }
            });
    }

    [Fact]
    public async Task Generic_ExecuteExAsync_WithObjectVars_ForwardsDictionary_AndReturnsResult()
    {
        // Arrange
        var vars = new TestVars { Name = "Y", Number = 2 };

        var fakeExecutor = A.Fake<IProcessExecutor<string>>();
        var fakeProcess = A.Fake<IProcess>();
        var expected = new ProcessExecutionResult<string?>(fakeProcess, "data");

        IDictionary<string, object?>? capturedVars = null;
        A.CallTo(() => fakeExecutor.ExecuteExAsync(A<IDictionary<string, object?>>._))
            .Invokes(call => capturedVars = call.GetArgument<IDictionary<string, object?>>(0))
            .Returns(Task.FromResult(expected));

        // Act
        var result = await ProcessExecutorGenericExtensions.ExecuteExAsync(fakeExecutor, vars);

        // Assert
        result
            .Should().BeSameAs(expected);

        result.Value
            .Should().Be("data");

        result.Process
            .Should().BeSameAs(fakeProcess);

        capturedVars
            .Should().NotBeNull()
            .And.HaveCount(2)
            .And.BeEquivalentTo(new Dictionary<string, object?>()
            {
                { "Name", "Y" },
                { "Number", 2 }
            });
    }

    [Fact]
    public void NonGeneric_Execute_WithObjectVars_ForwardsDictionary()
    {
        // Arrange
        var vars = new TestVars { Name = "NG", Number = 9 };

        var fakeExecutor = A.Fake<IProcessExecutor>();

        IDictionary<string, object?>? capturedVars = null;
        A.CallTo(() => fakeExecutor.Execute(A<IDictionary<string, object?>>._))
            .Invokes(call => capturedVars = call.GetArgument<IDictionary<string, object?>>(0));

        // Act
        ProcessExecutorExtensions.Execute(fakeExecutor, vars);

        // Assert
        capturedVars
            .Should().NotBeNull()
            .And.HaveCount(2)
            .And.BeEquivalentTo(new Dictionary<string, object?>()
            {
                { "Name", "NG" },
                { "Number", 9 }
            });
    }

    [Fact]
    public async Task NonGeneric_ExecuteAsync_WithObjectVars_ForwardsDictionary()
    {
        // Arrange
        var vars = new TestVars { Name = "NGA", Number = 10 };

        var fakeExecutor = A.Fake<IProcessExecutor>();

        IDictionary<string, object?>? capturedVars = null;
        A.CallTo(() => fakeExecutor.ExecuteAsync(A<IDictionary<string, object?>>._))
            .Invokes(call => capturedVars = call.GetArgument<IDictionary<string, object?>>(0))
            .Returns(Task.CompletedTask);

        // Act
        await ProcessExecutorExtensions.ExecuteAsync(fakeExecutor, vars);

        // Assert
        capturedVars
            .Should().NotBeNull()
            .And.HaveCount(2)
            .And.BeEquivalentTo(new Dictionary<string, object?>()
            {
                { "Name", "NGA" },
                { "Number", 10 }
            });
    }

    [Fact]
    public void NonGeneric_ExecuteEx_WithObjectVars_ForwardsDictionary_AndReturnsProcess()
    {
        // Arrange
        var vars = new TestVars { Name = "P", Number = 3 };

        var fakeExecutor = A.Fake<IProcessExecutor>();
        var fakeProcess = A.Fake<IProcess>();

        IDictionary<string, object?>? capturedVars = null;
        A.CallTo(() => fakeExecutor.ExecuteEx(A<IDictionary<string, object?>>._))
            .Invokes(call => capturedVars = call.GetArgument<IDictionary<string, object?>>(0))
            .Returns(fakeProcess);

        // Act
        var process = ProcessExecutorExtensions.ExecuteEx(fakeExecutor, vars);

        // Assert
        process
            .Should().BeSameAs(fakeProcess);

        capturedVars
            .Should().NotBeNull()
            .And.HaveCount(2)
            .And.BeEquivalentTo(new Dictionary<string, object?>()
            {
                { "Name", "P" },
                { "Number", 3 }
            });
    }

    [Fact]
    public async Task NonGeneric_ExecuteExAsync_WithObjectVars_ForwardsDictionary_AndReturnsProcess()
    {
        // Arrange
        var vars = new TestVars { Name = "PA", Number = 4 };

        var fakeExecutor = A.Fake<IProcessExecutor>();
        var fakeProcess = A.Fake<IProcess>();

        IDictionary<string, object?>? capturedVars = null;
        A.CallTo(() => fakeExecutor.ExecuteExAsync(A<IDictionary<string, object?>>._))
            .Invokes(call => capturedVars = call.GetArgument<IDictionary<string, object?>>(0))
            .Returns(Task.FromResult(fakeProcess));

        // Act
        var process = await ProcessExecutorExtensions.ExecuteExAsync(fakeExecutor, vars);

        // Assert
        process
            .Should().BeSameAs(fakeProcess);

        capturedVars
            .Should().NotBeNull()
            .And.HaveCount(2)
            .And.BeEquivalentTo(new Dictionary<string, object?>()
            {
                { "Name", "PA" },
                { "Number", 4 }
            });
    }

    [Fact]
    public void NonGeneric_ExecuteAndReturnExitCode_WithObjectVars_ForwardsDictionary_AndReturnsExitCode()
    {
        // Arrange
        var vars = new TestVars { Name = "E", Number = 5 };

        var fakeExecutor = A.Fake<IProcessExecutor>();

        IDictionary<string, object?>? capturedVars = null;
        A.CallTo(() => fakeExecutor.ExecuteAndReturnExitCode(A<IDictionary<string, object?>>._))
            .Invokes(call => capturedVars = call.GetArgument<IDictionary<string, object?>>(0))
            .Returns(77);

        // Act
        var exitCode = ProcessExecutorExtensions.ExecuteAndReturnExitCode(fakeExecutor, vars);

        // Assert
        exitCode
            .Should().Be(77);

        capturedVars
            .Should().NotBeNull()
            .And.HaveCount(2)
            .And.BeEquivalentTo(new Dictionary<string, object?>()
            {
                { "Name", "E" },
                { "Number", 5 }
            });
    }

    [Fact]
    public async Task
        NonGeneric_ExecuteAndReturnExitCodeAsync_WithObjectVars_ForwardsDictionary_AndReturnsExitCode()
    {
        // Arrange
        var vars = new TestVars { Name = "EA", Number = 6 };

        var fakeExecutor = A.Fake<IProcessExecutor>();

        IDictionary<string, object?>? capturedVars = null;
        A.CallTo(() => fakeExecutor.ExecuteAndReturnExitCodeAsync(A<IDictionary<string, object?>>._))
            .Invokes(call => capturedVars = call.GetArgument<IDictionary<string, object?>>(0))
            .Returns(Task.FromResult(99));

        // Act
        var exitCode = await ProcessExecutorExtensions.ExecuteAndReturnExitCodeAsync(fakeExecutor, vars);

        // Assert
        exitCode
            .Should().Be(99);

        capturedVars
            .Should().NotBeNull()
            .And.HaveCount(2)
            .And.BeEquivalentTo(new Dictionary<string, object?>()
            {
                { "Name", "EA" },
                { "Number", 6 }
            });
    }
}

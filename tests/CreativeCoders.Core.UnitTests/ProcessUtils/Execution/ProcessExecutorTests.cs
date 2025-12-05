using System.Diagnostics;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using CreativeCoders.ProcessUtils;
using CreativeCoders.ProcessUtils.Execution;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ProcessUtils.Execution;

#nullable enable

[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class ProcessExecutorTests
{
    [Fact]
    public void Execute_StartsProcess_WithConfiguredStartInfo_AndWaitsForExit()
    {
        // Arrange
        const string fileName = "my-tool";
        var args = new[] { "arg1", "arg2" };

        var info = new ProcessExecutorInfo(fileName, args, false);

        var fakeProcess = A.Fake<IProcess>();

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        executor.Execute();

        // Assert
        capturedStartInfo
            .Should()
            .NotBeNull();

        capturedStartInfo!.FileName
            .Should()
            .Be(fileName);

        capturedStartInfo.Arguments
            .Should()
            .Be(string.Join(" ", args));

        // Defaults from ProcessExecutorBase
        capturedStartInfo.RedirectStandardOutput
            .Should()
            .BeTrue();

        capturedStartInfo.RedirectStandardError
            .Should()
            .BeTrue();

        capturedStartInfo.RedirectStandardInput
            .Should()
            .BeTrue();

        capturedStartInfo.UseShellExecute
            .Should()
            .BeFalse();

        capturedStartInfo.CreateNoWindow
            .Should()
            .BeTrue();

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Execute_WithArgs_StartsProcess_UsesProvidedArgs_AndWaitsForExit()
    {
        // Arrange
        const string fileName = "my-tool";
        var defaultArgs = new[] { "x", "y" };
        var providedArgs = new[] { "argA", "argB", "argC" };

        var info = new ProcessExecutorInfo(fileName, defaultArgs, false);

        var fakeProcess = A.Fake<IProcess>();

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        executor.Execute(providedArgs);

        // Assert
        capturedStartInfo
            .Should()
            .NotBeNull();

        capturedStartInfo!.FileName
            .Should()
            .Be(fileName);

        capturedStartInfo.Arguments
            .Should()
            .Be(string.Join(" ", providedArgs));

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Execute_WithPlaceholderVars_StartsProcess_ReplacesPlaceholders_AndWaitsForExit()
    {
        // Arrange
        const string fileName = "my-tool";
        var argsWithPlaceholders = new[] { "run", "--value", "{{val}}" };

        var info = new ProcessExecutorInfo(fileName, argsWithPlaceholders, false);

        var fakeProcess = A.Fake<IProcess>();

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        executor.Execute(new Dictionary<string, object?> { ["val"] = 42 });

        // Assert
        capturedStartInfo
            .Should()
            .NotBeNull();

        capturedStartInfo!.FileName
            .Should()
            .Be(fileName);

        capturedStartInfo.Arguments
            .Should()
            .Be("run --value 42");

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ExecuteEx_StartsProcess_WithConfiguredStartInfo_AndWaitsForExit()
    {
        // Arrange
        const string fileName = "my-tool";
        var args = new[] { "arg1", "arg2" };

        var info = new ProcessExecutorInfo(fileName, args, false);

        var fakeProcess = A.Fake<IProcess>();

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        using var process = executor.ExecuteEx();

        // Assert
        process
            .Should()
            .BeSameAs(fakeProcess);

        capturedStartInfo
            .Should()
            .NotBeNull();

        capturedStartInfo!.FileName
            .Should()
            .Be(fileName);

        capturedStartInfo.Arguments
            .Should()
            .Be(string.Join(" ", args));

        // Defaults from ProcessExecutorBase
        capturedStartInfo.RedirectStandardOutput
            .Should()
            .BeTrue();

        capturedStartInfo.RedirectStandardError
            .Should()
            .BeTrue();

        capturedStartInfo.RedirectStandardInput
            .Should()
            .BeTrue();

        capturedStartInfo.UseShellExecute
            .Should()
            .BeFalse();

        capturedStartInfo.CreateNoWindow
            .Should()
            .BeTrue();

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustNotHaveHappened();
    }

    [Fact]
    public void ExecuteEx_WithArgs_StartsProcess_UsesProvidedArgs_AndReturnsProcess()
    {
        // Arrange
        const string fileName = "tool-ex";
        var defaultArgs = new[] { "d1" };
        var providedArgs = new[] { "n1", "n2" };

        var info = new ProcessExecutorInfo(fileName, defaultArgs, false);

        var fakeProcess = A.Fake<IProcess>();

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        using var process = executor.ExecuteEx(providedArgs);

        // Assert
        process
            .Should()
            .BeSameAs(fakeProcess);

        capturedStartInfo
            .Should()
            .NotBeNull();

        capturedStartInfo!.Arguments
            .Should()
            .Be(string.Join(" ", providedArgs));

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustNotHaveHappened();
    }

    [Fact]
    public void ExecuteEx_WithPlaceholderVars_StartsProcess_ReplacesPlaceholders_AndReturnsProcess()
    {
        // Arrange
        const string fileName = "tool-ex";
        var argsWithPlaceholders = new[] { "run", "{{name}}" };

        var info = new ProcessExecutorInfo(fileName, argsWithPlaceholders, false);

        var fakeProcess = A.Fake<IProcess>();

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        using var process = executor.ExecuteEx(new Dictionary<string, object?> { ["name"] = "John" });

        // Assert
        process.Should().BeSameAs(fakeProcess);
        capturedStartInfo!.Arguments.Should().Be("run John");
        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustNotHaveHappened();
    }

    [Fact]
    public async Task ExecuteAsync_StartsProcess_WithConfiguredStartInfo_AndWaitsForExitAsync()
    {
        // Arrange
        var fileName = "my-tool-async";
        var args = new[] { "a", "b" };

        var info = new ProcessExecutorInfo(fileName, args, false);

        var fakeProcess = A.Fake<IProcess>();

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        // Make WaitForExitAsync complete immediately
        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._))
            .Returns(Task.CompletedTask);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        await executor.ExecuteAsync();

        // Assert
        capturedStartInfo
            .Should()
            .NotBeNull();

        capturedStartInfo!.FileName
            .Should()
            .Be(fileName);

        capturedStartInfo.Arguments
            .Should()
            .Be(string.Join(" ", args));

        // Defaults from ProcessExecutorBase
        capturedStartInfo.RedirectStandardOutput
            .Should()
            .BeTrue();

        capturedStartInfo.RedirectStandardError
            .Should()
            .BeTrue();

        capturedStartInfo.RedirectStandardInput
            .Should()
            .BeTrue();

        capturedStartInfo.UseShellExecute
            .Should()
            .BeFalse();

        capturedStartInfo.CreateNoWindow
            .Should()
            .BeTrue();

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ExecuteExAsync_StartsProcess_WithConfiguredStartInfo_AndWaitsForExitAsync()
    {
        // Arrange
        var fileName = "my-tool-async";
        var args = new[] { "a", "b" };

        var info = new ProcessExecutorInfo(fileName, args, false);

        var fakeProcess = A.Fake<IProcess>();

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        // Make WaitForExitAsync complete immediately
        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._))
            .Returns(Task.CompletedTask);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        using var process = await executor.ExecuteExAsync();

        // Assert
        process
            .Should()
            .BeSameAs(fakeProcess);

        capturedStartInfo
            .Should()
            .NotBeNull();

        capturedStartInfo!.FileName
            .Should()
            .Be(fileName);

        capturedStartInfo.Arguments
            .Should()
            .Be(string.Join(" ", args));

        // Defaults from ProcessExecutorBase
        capturedStartInfo.RedirectStandardOutput
            .Should()
            .BeTrue();

        capturedStartInfo.RedirectStandardError
            .Should()
            .BeTrue();

        capturedStartInfo.RedirectStandardInput
            .Should()
            .BeTrue();

        capturedStartInfo.UseShellExecute
            .Should()
            .BeFalse();

        capturedStartInfo.CreateNoWindow
            .Should()
            .BeTrue();

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustNotHaveHappened();
    }

    [Fact]
    public async Task ExecuteAsync_WithArgs_StartsProcess_UsesProvidedArgs_AndWaitsForExitAsync()
    {
        // Arrange
        var fileName = "my-tool-async";
        var defaultArgs = new[] { "d" };
        var providedArgs = new[] { "x", "y" };

        var info = new ProcessExecutorInfo(fileName, defaultArgs, false);

        var fakeProcess = A.Fake<IProcess>();

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._))
            .Returns(Task.CompletedTask);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        await executor.ExecuteAsync(providedArgs);

        // Assert
        capturedStartInfo!.Arguments
            .Should().Be(string.Join(" ", providedArgs));

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task
        ExecuteAsync_WithPlaceholderVars_StartsProcess_ReplacesPlaceholders_AndWaitsForExitAsync()
    {
        // Arrange
        var fileName = "my-tool-async";
        var argsWithPlaceholders = new[] { "go", "--n", "{{num}}" };

        var info = new ProcessExecutorInfo(fileName, argsWithPlaceholders, false);

        var fakeProcess = A.Fake<IProcess>();

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._))
            .Returns(Task.CompletedTask);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        await executor.ExecuteAsync(new Dictionary<string, object?> { ["num"] = 7 });

        // Assert
        capturedStartInfo!.Arguments
            .Should().Be("go --n 7");

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ExecuteExAsync_WithArgs_StartsProcess_UsesProvidedArgs_AndReturnsProcess()
    {
        // Arrange
        var fileName = "my-tool-async";
        var defaultArgs = new[] { "d" };
        var providedArgs = new[] { "x", "y" };

        var info = new ProcessExecutorInfo(fileName, defaultArgs, false);

        var fakeProcess = A.Fake<IProcess>();

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._))
            .Returns(Task.CompletedTask);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        using var process = await executor.ExecuteExAsync(providedArgs);

        // Assert
        process
            .Should().BeSameAs(fakeProcess);

        capturedStartInfo!.Arguments
            .Should().Be(string.Join(" ", providedArgs));

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustNotHaveHappened();
    }

    [Fact]
    public async Task
        ExecuteExAsync_WithPlaceholderVars_StartsProcess_ReplacesPlaceholders_AndReturnsProcess()
    {
        // Arrange
        var fileName = "my-tool-async";
        var argsWithPlaceholders = new[] { "go", "{{word}}" };

        var info = new ProcessExecutorInfo(fileName, argsWithPlaceholders, false);

        var fakeProcess = A.Fake<IProcess>();

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._))
            .Returns(Task.CompletedTask);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        using var process =
            await executor.ExecuteExAsync(new Dictionary<string, object?> { ["word"] = "ok" });

        // Assert
        process
            .Should().BeSameAs(fakeProcess);

        capturedStartInfo!.Arguments
            .Should().Be("go ok");

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustNotHaveHappened();
    }

    [Fact]
    public void ExecuteAndReturnExitCode_StartsProcess_WithConfiguredStartInfo_ReturnsExitCode()
    {
        // Arrange
        const string fileName = "my-tool";
        var args = new[] { "arg1", "arg2" };
        const int expectedExitCode = 1235;

        var info = new ProcessExecutorInfo(fileName, args, false);

        var fakeProcess = A.Fake<IProcess>();

        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Returns(fakeProcess);

        A.CallTo(() => fakeProcess.ExitCode)
            .Returns(expectedExitCode);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        var exitCode = executor.ExecuteAndReturnExitCode();

        // Assert
        exitCode
            .Should()
            .Be(expectedExitCode);

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ExecuteAndReturnExitCodeAsync_StartsProcess_WithConfiguredStartInfo_ReturnsExitCode()
    {
        // Arrange
        var fileName = "my-tool-async";
        var args = new[] { "a", "b" };
        const int expectedExitCode = 1235;

        var info = new ProcessExecutorInfo(fileName, args, false);

        var fakeProcess = A.Fake<IProcess>();

        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Returns(fakeProcess);

        // Make WaitForExitAsync complete immediately
        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._))
            .Returns(Task.CompletedTask);

        A.CallTo(() => fakeProcess.ExitCode)
            .Returns(expectedExitCode);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        var exitCode = await executor.ExecuteAndReturnExitCodeAsync();

        // Assert
        exitCode
            .Should()
            .Be(expectedExitCode);

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ExecuteAndReturnExitCode_WithArgs_UsesProvidedArgs_AndReturnsExitCode()
    {
        // Arrange
        const string fileName = "my-tool";
        var defaultArgs = new[] { "d" };
        var providedArgs = new[] { "a1", "a2" };
        const int expectedExitCode = 55;

        var info = new ProcessExecutorInfo(fileName, defaultArgs, false);

        var fakeProcess = A.Fake<IProcess>();
        A.CallTo(() => fakeProcess.ExitCode).Returns(expectedExitCode);

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        var exitCode = executor.ExecuteAndReturnExitCode(providedArgs);

        // Assert
        exitCode.Should().Be(expectedExitCode);
        capturedStartInfo!.Arguments.Should().Be(string.Join(" ", providedArgs));
        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ExecuteAndReturnExitCode_WithPlaceholderVars_ReplacesPlaceholders_AndReturnsExitCode()
    {
        // Arrange
        const string fileName = "my-tool";
        var argsWithPlaceholders = new[] { "do", "{{n}}" };
        const int expectedExitCode = 77;

        var info = new ProcessExecutorInfo(fileName, argsWithPlaceholders, false);

        var fakeProcess = A.Fake<IProcess>();
        A.CallTo(() => fakeProcess.ExitCode).Returns(expectedExitCode);

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        var exitCode = executor.ExecuteAndReturnExitCode(new Dictionary<string, object?> { ["n"] = 9 });

        // Assert
        exitCode.Should().Be(expectedExitCode);
        capturedStartInfo!.Arguments.Should().Be("do 9");
        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ExecuteAndReturnExitCodeAsync_WithArgs_UsesProvidedArgs_AndReturnsExitCode()
    {
        // Arrange
        var fileName = "my-tool-async";
        var defaultArgs = new[] { "d" };
        var providedArgs = new[] { "a1", "a2" };
        const int expectedExitCode = 101;

        var info = new ProcessExecutorInfo(fileName, defaultArgs, false);

        var fakeProcess = A.Fake<IProcess>();
        A.CallTo(() => fakeProcess.ExitCode).Returns(expectedExitCode);

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._))
            .Returns(Task.CompletedTask);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        var exitCode = await executor.ExecuteAndReturnExitCodeAsync(providedArgs);

        // Assert
        exitCode.Should().Be(expectedExitCode);
        capturedStartInfo!.Arguments.Should().Be(string.Join(" ", providedArgs));
        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task
        ExecuteAndReturnExitCodeAsync_WithPlaceholderVars_ReplacesPlaceholders_AndReturnsExitCode()
    {
        // Arrange
        var fileName = "my-tool-async";
        var argsWithPlaceholders = new[] { "do", "{{n}}" };
        const int expectedExitCode = 202;

        var info = new ProcessExecutorInfo(fileName, argsWithPlaceholders, false);

        var fakeProcess = A.Fake<IProcess>();
        A.CallTo(() => fakeProcess.ExitCode).Returns(expectedExitCode);

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._))
            .Returns(Task.CompletedTask);

        var executor = new ProcessExecutor(info, processFactory);

        // Act
        var exitCode =
            await executor.ExecuteAndReturnExitCodeAsync(new Dictionary<string, object?> { ["n"] = 3 });

        // Assert
        exitCode.Should().Be(expectedExitCode);
        capturedStartInfo!.Arguments.Should().Be("do 3");
        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }
}

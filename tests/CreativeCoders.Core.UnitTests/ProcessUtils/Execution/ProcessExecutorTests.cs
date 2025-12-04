using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using CreativeCoders.ProcessUtils;
using CreativeCoders.ProcessUtils.Execution;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ProcessUtils.Execution;

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
}

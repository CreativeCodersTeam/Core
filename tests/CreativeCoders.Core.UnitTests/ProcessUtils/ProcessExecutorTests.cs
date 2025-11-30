using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.ProcessUtils;
using CreativeCoders.ProcessUtils.Execution;
using FakeItEasy;
using AwesomeAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ProcessUtils;

[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class ProcessExecutorTests
{
    [Fact]
    public void Execute_StartsProcess_WithConfiguredStartInfo_AndWaitsForExit()
    {
        // Arrange
        const string fileName = "my-tool";
        var args = new[] {"arg1", "arg2"};

        var info = new ProcessExecutorInfo(fileName, args);

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
    public async Task ExecuteAsync_StartsProcess_WithConfiguredStartInfo_AndWaitsForExitAsync()
    {
        // Arrange
        var fileName = "my-tool-async";
        var args = new[] {"a", "b"};

        var info = new ProcessExecutorInfo(fileName, args);

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
}

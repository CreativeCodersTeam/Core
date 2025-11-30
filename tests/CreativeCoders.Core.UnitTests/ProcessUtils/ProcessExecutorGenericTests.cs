using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using CreativeCoders.ProcessUtils;
using CreativeCoders.ProcessUtils.Execution;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ProcessUtils;

public class ProcessExecutorGenericTests
{
    [Fact]
    public void Execute_ReadsOutput_AndParsesResult()
    {
        // Arrange
        var fileName = "echo";
        var args = new[] {"hello"};

        // Parser
        var parser = A.Fake<IProcessOutputParser<int>>();
        A.CallTo(() => parser.ParseOutput("42\n"))
            .Returns(42);

        var info = new ProcessExecutorInfo<int>(fileName, args, parser);

        // Fake process with output
        var fakeProcess = A.Fake<IProcess>();

        var outputStream = new MemoryStream(Encoding.UTF8.GetBytes("42\n"));
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Returns(fakeProcess);

        var executor = new ProcessExecutor<int>(info, processFactory);

        // Act
        var result = executor.Execute();

        // Assert
        result
            .Should()
            .Be(42);

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => parser.ParseOutput("42\n")).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ExecuteAsync_ReadsOutputAsync_AndParsesResult()
    {
        // Arrange
        var fileName = "echo";
        var args = new[] {"hello"};

        var parser = A.Fake<IProcessOutputParser<string>>();
        A.CallTo(() => parser.ParseOutput("hello world"))
            .Returns("hello world");

        var info = new ProcessExecutorInfo<string>(fileName, args, parser);

        var fakeProcess = A.Fake<IProcess>();

        var outputStream = new MemoryStream(Encoding.UTF8.GetBytes("hello world"));
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._))
            .Returns(Task.CompletedTask);

        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Returns(fakeProcess);

        var executor = new ProcessExecutor<string>(info, processFactory);

        // Act
        var result = await executor.ExecuteAsync();

        // Assert
        result
            .Should()
            .Be("hello world");
        A.CallTo(() => parser.ParseOutput("hello world")).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }
}

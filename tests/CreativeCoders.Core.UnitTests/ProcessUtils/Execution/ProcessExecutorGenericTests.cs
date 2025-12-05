using System.Diagnostics;
using System.IO;
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
public class ProcessExecutorGenericTests
{
    [Fact]
    public void Execute_ReadsOutput_AndParsesResult()
    {
        // Arrange
        const string fileName = "echo";
        var args = new[] { "hello" };

        // Parser
        var parser = A.Fake<IProcessOutputParser<int>>();
        A.CallTo(() => parser.ParseOutput("42\n"))
            .Returns(42);

        var info = new ProcessExecutorInfo<int>(fileName, args, parser);

        // Fake process with output
        var fakeProcess = A.Fake<IProcess>();

        var outputStream = new MemoryStream("42\n"u8.ToArray());
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
    public void ExecuteEx_ReadsOutput_AndParsesResult()
    {
        // Arrange
        const string fileName = "echo";
        var args = new[] { "hello" };

        // Parser
        var parser = A.Fake<IProcessOutputParser<int>>();
        A.CallTo(() => parser.ParseOutput("42\n"))
            .Returns(42);

        var info = new ProcessExecutorInfo<int>(fileName, args, parser);

        // Fake process with output
        var fakeProcess = A.Fake<IProcess>();

        var outputStream = new MemoryStream("42\n"u8.ToArray());
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Returns(fakeProcess);

        var executor = new ProcessExecutor<int>(info, processFactory);

        // Act
        var result = executor.ExecuteEx();

        // Assert
        result.Value
            .Should()
            .Be(42);

        result.Process
            .Should()
            .BeSameAs(fakeProcess);

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => parser.ParseOutput("42\n")).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustNotHaveHappened();

        result.Dispose();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ExecuteAsync_ReadsOutputAsync_AndParsesResult()
    {
        // Arrange
        var fileName = "echo";
        var args = new[] { "hello" };

        var parser = A.Fake<IProcessOutputParser<string>>();
        A.CallTo(() => parser.ParseOutput("hello world"))
            .Returns("hello world");

        var info = new ProcessExecutorInfo<string>(fileName, args, parser);

        var fakeProcess = A.Fake<IProcess>();

        var outputStream = new MemoryStream("hello world"u8.ToArray());
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

    [Fact]
    public async Task ExecuteExAsync_ReadsOutput_AndParsesResult()
    {
        // Arrange
        const string fileName = "echo";
        var args = new[] { "hello" };

        // Parser
        var parser = A.Fake<IProcessOutputParser<int>>();
        A.CallTo(() => parser.ParseOutput("42\n"))
            .Returns(42);

        var info = new ProcessExecutorInfo<int>(fileName, args, parser);

        // Fake process with output
        var fakeProcess = A.Fake<IProcess>();

        var outputStream = new MemoryStream("42\n"u8.ToArray());
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Returns(fakeProcess);

        var executor = new ProcessExecutor<int>(info, processFactory);

        // Act
        var result = await executor.ExecuteExAsync();

        // Assert
        result.Value
            .Should()
            .Be(42);

        result.Process
            .Should()
            .BeSameAs(fakeProcess);

        A.CallTo(() => fakeProcess.WaitForExitAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => parser.ParseOutput("42\n")).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustNotHaveHappened();

        result.Dispose();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Execute_WithArgs_ReadsOutput_ParsesResult_AndUsesProvidedArgs()
    {
        // Arrange
        const string fileName = "calc";
        var defaultArgs = new[] { "1", "+", "1" };
        var providedArgs = new[] { "6", "+", "7" };

        var parser = A.Fake<IProcessOutputParser<int>>();
        A.CallTo(() => parser.ParseOutput("13"))
            .Returns(13);

        var info = new ProcessExecutorInfo<int>(fileName, defaultArgs, parser);

        var fakeProcess = A.Fake<IProcess>();
        var outputStream = new MemoryStream("13"u8.ToArray());
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor<int>(info, processFactory);

        // Act
        var result = executor.Execute(providedArgs);

        // Assert
        result
            .Should().Be(13);

        capturedStartInfo
            .Should().NotBeNull();

        capturedStartInfo!.Arguments
            .Should().Be(string.Join(" ", providedArgs));

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => parser.ParseOutput("13")).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Execute_WithPlaceholderVars_ReadsOutput_ParsesResult_AndReplacesPlaceholders()
    {
        // Arrange
        const string fileName = "calc";
        var argsWithPlaceholders = new[] { "{{a}}", "+", "{{b}}" };

        var parser = A.Fake<IProcessOutputParser<int>>();
        A.CallTo(() => parser.ParseOutput("11"))
            .Returns(11);

        var info = new ProcessExecutorInfo<int>(fileName, argsWithPlaceholders, parser);

        var fakeProcess = A.Fake<IProcess>();
        var outputStream = new MemoryStream("11"u8.ToArray());
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor<int>(info, processFactory);

        // Act
        var result = executor.Execute(new Dictionary<string, object?> { ["a"] = 5, ["b"] = 6 });

        // Assert
        result
            .Should().Be(11);

        capturedStartInfo
            .Should().NotBeNull();

        capturedStartInfo!.Arguments
            .Should().Be("5 + 6");

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => parser.ParseOutput("11")).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ExecuteEx_WithArgs_ReadsOutput_ParsesResult_UsesProvidedArgs_AndReturnsProcess()
    {
        // Arrange
        const string fileName = "echo";
        var defaultArgs = new[] { "x" };
        var providedArgs = new[] { "hello", "world" };

        var parser = A.Fake<IProcessOutputParser<string>>();
        A.CallTo(() => parser.ParseOutput("ok"))
            .Returns("ok");

        var info = new ProcessExecutorInfo<string>(fileName, defaultArgs, parser);

        var fakeProcess = A.Fake<IProcess>();
        var outputStream = new MemoryStream("ok"u8.ToArray());
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor<string>(info, processFactory);

        // Act
        var result = executor.ExecuteEx(providedArgs);

        // Assert
        result.Value
            .Should().Be("ok");

        result.Process
            .Should().BeSameAs(fakeProcess);

        capturedStartInfo
            .Should().NotBeNull();

        capturedStartInfo!.Arguments
            .Should().Be(string.Join(" ", providedArgs));

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => parser.ParseOutput("ok")).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustNotHaveHappened();

        result.Dispose();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void
        ExecuteEx_WithPlaceholderVars_ReadsOutput_ParsesResult_ReplacesPlaceholders_AndReturnsProcess()
    {
        // Arrange
        const string fileName = "echo";
        var argsWithPlaceholders = new[] { "say", "{{word}}" };

        var parser = A.Fake<IProcessOutputParser<string>>();
        A.CallTo(() => parser.ParseOutput("done"))
            .Returns("done");

        var info = new ProcessExecutorInfo<string>(fileName, argsWithPlaceholders, parser);

        var fakeProcess = A.Fake<IProcess>();
        var outputStream = new MemoryStream("done"u8.ToArray());
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor<string>(info, processFactory);

        // Act
        var result = executor.ExecuteEx(new Dictionary<string, object?> { ["word"] = "hi" });

        // Assert
        result.Value
            .Should().Be("done");

        result.Process
            .Should().BeSameAs(fakeProcess);

        capturedStartInfo
            .Should().NotBeNull();

        capturedStartInfo!.Arguments
            .Should().Be("say hi");

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => parser.ParseOutput("done")).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustNotHaveHappened();

        result.Dispose();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ExecuteAsync_WithArgs_ReadsOutput_ParsesResult_AndUsesProvidedArgs()
    {
        // Arrange
        const string fileName = "echo";
        var defaultArgs = new[] { "a" };
        var providedArgs = new[] { "1", "2" };

        var parser = A.Fake<IProcessOutputParser<string>>();
        A.CallTo(() => parser.ParseOutput("res"))
            .Returns("res");

        var info = new ProcessExecutorInfo<string>(fileName, defaultArgs, parser);

        var fakeProcess = A.Fake<IProcess>();
        var outputStream = new MemoryStream("res"u8.ToArray());
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);
        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._))
            .Returns(Task.CompletedTask);

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor<string>(info, processFactory);

        // Act
        var result = await executor.ExecuteAsync(providedArgs);

        // Assert
        result
            .Should().Be("res");

        capturedStartInfo
            .Should().NotBeNull();

        capturedStartInfo!.Arguments
            .Should().Be(string.Join(" ", providedArgs));

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => parser.ParseOutput("res")).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ExecuteAsync_WithPlaceholderVars_ReadsOutput_ParsesResult_AndReplacesPlaceholders()
    {
        // Arrange
        const string fileName = "echo";
        var argsWithPlaceholders = new[] { "{{x}}", "{{y}}" };

        var parser = A.Fake<IProcessOutputParser<string>>();
        A.CallTo(() => parser.ParseOutput("ok"))
            .Returns("ok");

        var info = new ProcessExecutorInfo<string>(fileName, argsWithPlaceholders, parser);

        var fakeProcess = A.Fake<IProcess>();
        var outputStream = new MemoryStream("ok"u8.ToArray());
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);
        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._))
            .Returns(Task.CompletedTask);

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor<string>(info, processFactory);

        // Act
        var result = await executor.ExecuteAsync(new Dictionary<string, object?>
            { ["x"] = "X", ["y"] = "Y" });

        // Assert
        result
            .Should().Be("ok");

        capturedStartInfo
            .Should().NotBeNull();

        capturedStartInfo!.Arguments
            .Should().Be("X Y");

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => parser.ParseOutput("ok")).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ExecuteExAsync_WithArgs_ReadsOutput_ParsesResult_UsesProvidedArgs_AndReturnsProcess()
    {
        // Arrange
        const string fileName = "echo";
        var defaultArgs = new[] { "d" };
        var providedArgs = new[] { "hey" };

        var parser = A.Fake<IProcessOutputParser<string>>();
        A.CallTo(() => parser.ParseOutput("value"))
            .Returns("value");

        var info = new ProcessExecutorInfo<string>(fileName, defaultArgs, parser);

        var fakeProcess = A.Fake<IProcess>();
        var outputStream = new MemoryStream("value"u8.ToArray());
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor<string>(info, processFactory);

        // Act
        var result = await executor.ExecuteExAsync(providedArgs);

        // Assert
        result.Value
            .Should().Be("value");

        result.Process
            .Should().BeSameAs(fakeProcess);

        capturedStartInfo
            .Should().NotBeNull();

        capturedStartInfo!.Arguments
            .Should().Be(string.Join(" ", providedArgs));

        A.CallTo(() => fakeProcess.WaitForExitAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => parser.ParseOutput("value")).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustNotHaveHappened();

        result.Dispose();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task
        ExecuteExAsync_WithPlaceholderVars_ReadsOutput_ParsesResult_ReplacesPlaceholders_AndReturnsProcess()
    {
        // Arrange
        const string fileName = "echo";
        var argsWithPlaceholders = new[] { "say", "{{t}}" };

        var parser = A.Fake<IProcessOutputParser<string>>();
        A.CallTo(() => parser.ParseOutput("rdy"))
            .Returns("rdy");

        var info = new ProcessExecutorInfo<string>(fileName, argsWithPlaceholders, parser);

        var fakeProcess = A.Fake<IProcess>();
        var outputStream = new MemoryStream("rdy"u8.ToArray());
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutor<string>(info, processFactory);

        // Act
        var result = await executor.ExecuteExAsync(new Dictionary<string, object?> { ["t"] = "token" });

        // Assert
        result.Value
            .Should().Be("rdy");

        result.Process
            .Should().BeSameAs(fakeProcess);

        capturedStartInfo
            .Should().NotBeNull();

        capturedStartInfo!.Arguments
            .Should().Be("say token");

        A.CallTo(() => fakeProcess.WaitForExitAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => parser.ParseOutput("rdy")).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustNotHaveHappened();

        result.Dispose();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }
}

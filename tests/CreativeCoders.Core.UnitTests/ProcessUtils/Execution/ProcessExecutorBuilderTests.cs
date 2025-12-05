using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using CreativeCoders.ProcessUtils;
using CreativeCoders.ProcessUtils.Execution;
using CreativeCoders.ProcessUtils.Execution.Parsers;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ProcessUtils.Execution;

/// <summary>
/// Tests for <see cref="ProcessExecutorBuilder"/> and <see cref="ProcessExecutorBuilder{T}"/>.
/// Verifies guard clauses, configuration behavior, and integration with <see cref="IProcessFactory"/>.
/// </summary>
[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class ProcessExecutorBuilderTests
{
    [Fact]
    public void Build_WithoutFileName_Throws()
    {
        // Arrange
        var processFactory = A.Fake<IProcessFactory>();
        var builder = new ProcessExecutorBuilder(processFactory);

        // Act
        var act = () => builder.Build();

        // Assert
        act
            .Should()
            .ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void Build_WithFileNameAndArgs_CreatesExecutor_ThatStartsProcess()
    {
        // Arrange
        const string fileName = "tool";
        var args = new[] { "x", "y" };

        var fakeProcess = A.Fake<IProcess>();

        var capturedStartInfo = default(ProcessStartInfo);
        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => capturedStartInfo = si)
            .Returns(fakeProcess);

        var executor = new ProcessExecutorBuilder(processFactory)
            .SetFileName(fileName)
            .SetArguments(args)
            .Build();

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

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void GenericBuild_WithoutFileName_Throws()
    {
        // Arrange
        var processFactory = A.Fake<IProcessFactory>();
        var builder = new ProcessExecutorBuilder<int>(processFactory)
            .SetOutputParser(A.Fake<IProcessOutputParser<int>>());

        // Act
        var act = () => builder.Build();

        // Assert
        act
            .Should()
            .ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void GenericBuild_WithoutParser_Throws()
    {
        // Arrange
        var processFactory = A.Fake<IProcessFactory>();
        var builder = new ProcessExecutorBuilder<int>(processFactory)
            .SetFileName("tool");

        // Act
        var act = () => builder.Build();

        // Assert
        act
            .Should()
            .ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void GenericBuild_WithCustomParser_AndConfigure_ReturnsParsedResult()
    {
        // Arrange
        const string fileName = "echo";
        var args = new[] { "val" };

        // Provide output "21" and configure parser to multiply by 2 -> 42
        var fakeProcess = A.Fake<IProcess>();
        var outputStream = new MemoryStream("21\n"u8.ToArray());
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Returns(fakeProcess);

        var builder = new ProcessExecutorBuilder<int>(processFactory)
            .SetFileName(fileName)
            .SetArguments(args)
            .SetOutputParser(new TestIntParser { Multiplier = 2 });

        var executor = builder.Build();

        // Act
        var result = executor.Execute();

        // Assert
        result
            .Should()
            .Be(42);

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void GenericBuild_WithCustomConfiguredParser_ReturnsParsedResult()
    {
        // Arrange
        const string fileName = "echo";
        var args = new[] { "val" };

        // Provide output "21" and configure parser to multiply by 2 -> 42
        var fakeProcess = A.Fake<IProcess>();
        var outputStream = new MemoryStream("21\n"u8.ToArray());
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Returns(fakeProcess);

        var builder = new ProcessExecutorBuilder<int>(processFactory)
            .SetFileName(fileName)
            .SetArguments(args)
            .SetOutputParser<TestIntParser>(x => x.Multiplier = 2);

        var executor = builder.Build();

        // Act
        var result = executor.Execute();

        // Assert
        result
            .Should()
            .Be(42);

        A.CallTo(() => fakeProcess.WaitForExit()).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GenericBuild_ReturnOutputAsText_ReturnsRawText()
    {
        // Arrange
        const string fileName = "echo";
        var args = new[] { "hello" };

        var fakeProcess = A.Fake<IProcess>();
        var outputStream = new MemoryStream("hello world"u8.ToArray());
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);
        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._))
            .Returns(Task.CompletedTask);

        var processFactory = A.Fake<IProcessFactory>();
        A.CallTo(() => processFactory.StartProcess(A<ProcessStartInfo>._))
            .Returns(fakeProcess);

        var executor = new ProcessExecutorBuilder<string>(processFactory)
            .SetFileName(fileName)
            .SetArguments(args)
            .SetOutputParser(new PassThroughProcessOutputParser())
            .Build();

        // Act
        var result = await executor.ExecuteAsync();

        // Assert
        result
            .Should()
            .Be("hello world");

        A.CallTo(() => fakeProcess.WaitForExitAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeProcess.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData("42")]
    [InlineData("Test, 12345")]
    [InlineData("  42  QWERTZ \n Line 1234")]
    public void Build_WhenTIsStringAndNoOutputParser_DoesNotThrowAndReturnsStringExecutor(
        string processOutput)
    {
        // Arrange
        var fakeProcessFactory = A.Fake<IProcessFactory>();
        var fakeProcess = A.Fake<IProcess>();

        var outputStream = new MemoryStream(Encoding.UTF8.GetBytes(processOutput));
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        var builder = new ProcessExecutorBuilder<string>(fakeProcessFactory)
            .SetFileName("dummy.exe"); // required to pass FileName check

        A.CallTo(() => fakeProcessFactory.StartProcess(A<ProcessStartInfo>._))
            .Returns(fakeProcess);

        // Act
        var executor = builder.Build();

        var executeResult = executor.Execute();

        // Assert
        executor
            .Should()
            .NotBeNull()
            .And
            .BeAssignableTo<IProcessExecutor<string>>();

        executeResult
            .Should()
            .Be(processOutput);
    }

    [Fact]
    public void BuildAndUseExecutor_ShouldThrowOnErrorSet_ThrowsExitCodeNotZero()
    {
        // Arrange
        var fakeProcessFactory = A.Fake<IProcessFactory>();
        var fakeProcess = A.Fake<IProcess>();

        A.CallTo(() => fakeProcess.ExitCode).Returns(1);
        A.CallTo(() => fakeProcessFactory.StartProcess(A<ProcessStartInfo>._))
            .Returns(fakeProcess);

        var outputStream = new MemoryStream(Encoding.UTF8.GetBytes("Process failed"));
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardError).Returns(reader);

        var executor = new ProcessExecutorBuilder(fakeProcessFactory)
            .SetFileName("dummy")
            .ShouldThrowOnError()
            .Build();

        // Act
        var act = () => executor.Execute();

        // Assert
        act
            .Should().Throw<ProcessExecutionFailedException>()
            .Which
            .Should().Match<ProcessExecutionFailedException>(x =>
                x.ExitCode == 1 && x.ErrorOutput == "Process failed");
    }

    [Fact]
    public async Task BuildAndUseExecutorAsync_ShouldThrowOnErrorSet_ThrowsExitCodeNotZero()
    {
        // Arrange
        var fakeProcessFactory = A.Fake<IProcessFactory>();
        var fakeProcess = A.Fake<IProcess>();

        A.CallTo(() => fakeProcess.ExitCode).Returns(1);
        A.CallTo(() => fakeProcessFactory.StartProcess(A<ProcessStartInfo>._))
            .Returns(fakeProcess);

        var outputStream = new MemoryStream(Encoding.UTF8.GetBytes("Process failed"));
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardError).Returns(reader);

        var executor = new ProcessExecutorBuilder(fakeProcessFactory)
            .SetFileName("dummy")
            .ShouldThrowOnError()
            .Build();

        // Act
        var act = async () => await executor.ExecuteAsync();

        // Assert
        (await act
                .Should().ThrowAsync<ProcessExecutionFailedException>())
            .Which
            .Should().Match<ProcessExecutionFailedException>(x =>
                x.ExitCode == 1 && x.ErrorOutput == "Process failed");
    }

    [Fact]
    public async Task BuildAndUseExecutorGenericAsync_ShouldThrowOnErrorSet_ThrowsExitCodeNotZero()
    {
        // Arrange
        var fakeProcessFactory = A.Fake<IProcessFactory>();
        var fakeProcess = A.Fake<IProcess>();

        A.CallTo(() => fakeProcess.ExitCode).Returns(1);
        A.CallTo(() => fakeProcessFactory.StartProcess(A<ProcessStartInfo>._))
            .Returns(fakeProcess);

        var outputErrorStream = new MemoryStream(Encoding.UTF8.GetBytes("Process failed"));
        var errorReader = new StreamReader(outputErrorStream);
        A.CallTo(() => fakeProcess.StandardError).Returns(errorReader);

        var outputStream = new MemoryStream(Encoding.UTF8.GetBytes("Process failed"));
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        var executor = new ProcessExecutorBuilder<string>(fakeProcessFactory)
            .SetFileName("dummy")
            .ShouldThrowOnError()
            .Build();

        // Act
        var act = async () => await executor.ExecuteAsync();

        // Assert
        (await act
                .Should().ThrowAsync<ProcessExecutionFailedException>())
            .Which
            .Should().Match<ProcessExecutionFailedException>(x =>
                x.ExitCode == 1 && x.ErrorOutput == "Process failed");
    }

    [Fact]
    public void BuildAndUseExecutor_SetupStartInfo_StartInfoIsConfigured()
    {
        // Arrange
        var fakeProcessFactory = A.Fake<IProcessFactory>();
        var fakeProcess = A.Fake<IProcess>();

        ProcessStartInfo startInfo = null;
        A.CallTo(() => fakeProcess.ExitCode).Returns(1);
        A.CallTo(() => fakeProcessFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => startInfo = si)
            .Returns(fakeProcess);

        var outputStream = new MemoryStream(Encoding.UTF8.GetBytes("Process failed"));
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardError).Returns(reader);

        var executor = new ProcessExecutorBuilder(fakeProcessFactory)
            .SetFileName("dummy")
            .SetupStartInfo(x =>
            {
                x.CreateNoWindow = false;
                x.RedirectStandardError = false;
                x.UseShellExecute = true;
            })
            .Build();

        // Act
        executor.Execute();

        // Assert
        startInfo
            .Should()
            .NotBeNull();

        startInfo.CreateNoWindow
            .Should()
            .BeFalse();

        startInfo.RedirectStandardError
            .Should()
            .BeFalse();

        startInfo.UseShellExecute
            .Should()
            .BeTrue();
    }

    [Fact]
    public void BuildAndUseGenericExecutor_SetupStartInfo_StartInfoIsConfigured()
    {
        // Arrange
        var fakeProcessFactory = A.Fake<IProcessFactory>();
        var fakeProcess = A.Fake<IProcess>();

        ProcessStartInfo startInfo = null;
        A.CallTo(() => fakeProcess.ExitCode).Returns(1);
        A.CallTo(() => fakeProcessFactory.StartProcess(A<ProcessStartInfo>._))
            .Invokes((ProcessStartInfo si) => startInfo = si)
            .Returns(fakeProcess);

        var outputStream = new MemoryStream(Encoding.UTF8.GetBytes("Process running"));
        var reader = new StreamReader(outputStream);
        A.CallTo(() => fakeProcess.StandardOutput).Returns(reader);

        var executor = new ProcessExecutorBuilder<string>(fakeProcessFactory)
            .SetFileName("dummy")
            .SetupStartInfo(x =>
            {
                x.CreateNoWindow = false;
                x.RedirectStandardError = false;
                x.UseShellExecute = true;
            })
            .Build();

        // Act
        executor.Execute();

        // Assert
        startInfo
            .Should()
            .NotBeNull();

        startInfo.CreateNoWindow
            .Should()
            .BeFalse();

        startInfo.RedirectStandardError
            .Should()
            .BeFalse();

        startInfo.UseShellExecute
            .Should()
            .BeTrue();
    }

    /// <summary>
    /// Simple test parser for integers; parses text to int and multiplies by <see cref="Multiplier"/>.
    /// </summary>
    private sealed class TestIntParser : IProcessOutputParser<int>
    {
        public int Multiplier { get; set; } = 1;

        public int ParseOutput(string output)
        {
            ArgumentNullException.ThrowIfNull(output);

            var parsed = int.Parse(output.Trim());
            return parsed * Multiplier;
        }
    }
}

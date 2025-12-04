using System;
using System.Diagnostics;
using AwesomeAssertions;
using CreativeCoders.ProcessUtils;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ProcessUtils;

/// <summary>
/// Tests for <see cref="DefaultProcessFactory"/> to verify process creation and start behavior.
/// Uses a harmless OS command for start tests to avoid side effects.
/// </summary>
public class DefaultProcessFactoryTests
{
    [Fact]
    public void CreateProcess_WithFileNameAndArgs_ReturnsDefaultProcess_WithConfiguredStartInfo()
    {
        // Arrange
        const string fileName = "/usr/bin/echo";
        var args = new[] { "hello", "world" };

        var factory = new DefaultProcessFactory();

        // Act
        using var process = factory.CreateProcess(fileName, args);

        // Assert
        process
            .Should()
            .NotBeNull();

        process
            .Should()
            .BeOfType<DefaultProcess>();

        // StartInfo must reflect provided values
        process.StartInfo.FileName
            .Should()
            .Be(fileName);

        process.StartInfo.Arguments
            .Should()
            .Be(string.Join(" ", args));
    }

    [Fact]
    public void CreateProcess_WithoutFileName_Throws()
    {
        // Arrange
        var factory = new DefaultProcessFactory();

        // Act
        var actNull = () => factory.CreateProcess(null!, Array.Empty<string>());
        var actEmpty = () => factory.CreateProcess(string.Empty, Array.Empty<string>());
        var actWhitespace = () => factory.CreateProcess(" ", Array.Empty<string>());

        // Assert
        actNull.Should().ThrowExactly<ArgumentException>();
        actEmpty.Should().ThrowExactly<ArgumentException>();
        actWhitespace.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void CreateProcess_WithoutParams_ReturnsDefaultProcess()
    {
        // Arrange
        var factory = new DefaultProcessFactory();

        // Act
        using var process = factory.CreateProcess();

        // Assert
        process
            .Should()
            .NotBeNull();

        process
            .Should()
            .BeOfType<DefaultProcess>();
    }

    [Fact]
    public void StartProcess_WithFileNameAndArgs_StartsProcess_AndReturnsWrapper()
    {
        // Arrange
        // Use a harmless command that exists on macOS/Linux; on unsupported OS just return
        if (!OperatingSystem.IsMacOS() && !OperatingSystem.IsLinux())
        {
            return;
        }

        const string fileName = "/usr/bin/true"; // exits immediately with code 0
        var factory = new DefaultProcessFactory();

        // Act
        using var process = factory.StartProcess(fileName, Array.Empty<string>());
        process.WaitForExit();

        // Assert
        process
            .Should()
            .BeOfType<DefaultProcess>();

        process.HasExited
            .Should()
            .BeTrue();

        process.ExitCode
            .Should()
            .Be(0);
    }

    [Fact]
    public void StartProcess_WithStartInfo_StartsProcess_AndReturnsWrapper()
    {
        // Arrange
        if (!OperatingSystem.IsMacOS() && !OperatingSystem.IsLinux())
        {
            return;
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = "/bin/echo",
            Arguments = "ok"
        };

        var factory = new DefaultProcessFactory();

        // Act
        using var process = factory.StartProcess(startInfo);
        process.WaitForExit();

        // Assert
        process
            .Should()
            .BeOfType<DefaultProcess>();

        process.HasExited
            .Should()
            .BeTrue();
    }
}

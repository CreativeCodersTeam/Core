using System;
using System.IO.Abstractions.TestingHelpers;
using CreativeCoders.Core.IO;
using AwesomeAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.IO;

public class PathSafetyTests
{
    [Theory]
    [InlineData("file.txt", "/base")]
    [InlineData("sub/file.txt", "/base")]
    [InlineData("./file.txt", "/base")]
    [InlineData("sub/../file.txt", "/base")]
    public void IsSafe_SafePaths_ReturnsTrue(string path, string basePath)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        var result = PathSafety.IsSafe(fileSystem.Path, path, basePath);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("../file.txt", "/base")]
    [InlineData("/etc/passwd", "/base")]
    [InlineData("sub/../../file.txt", "/base")]
    [InlineData("..", "/base")]
    [InlineData("../base_extra/file.txt", "/base")]
    public void IsSafe_UnsafePaths_ReturnsFalse(string path, string basePath)
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        // Act
        var result = PathSafety.IsSafe(fileSystem.Path, path, basePath);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EnsureSafe_SafePath_DoesNotThrow()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        const string path = "file.txt";
        const string basePath = "/base";

        // Act
        var act = () => PathSafety.EnsureSafe(fileSystem.Path, path, basePath);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureSafe_UnsafePath_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        const string path = "../file.txt";
        const string basePath = "/base";

        // Act
        var act = () => PathSafety.EnsureSafe(fileSystem.Path, path, basePath);

        // Assert
        act.Should().Throw<UnauthorizedAccessException>();
    }

    [Fact]
    public void IsSafe_PathHelperExtension_ReturnsTrue()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        const string path = "file.txt";
        const string basePath = "/base";

        // Act
        var result = fileSystem.Path.IsSafe(path, basePath);

        // Assert
        result.Should().BeTrue();
    }
}

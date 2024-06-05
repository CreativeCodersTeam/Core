using CreativeCoders.Core.IO;
using FluentAssertions;

namespace CreativeCoders.UnitTests.UnitTests;

[Collection("FileSys")]
public class StringAssertionsExtensionsTests
{
    [Fact]
    public void FileExists_WithExistingFile_ReturnsTrue()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();

        using var _ = new FileCleanUp(tempFile);

        // Act
        Action act = () => tempFile.Should().FileExists();

        // Assert
        act
            .Should()
            .NotThrow();
    }

    [Fact]
    public void FileExists_WithNonExistingFile_ThrowsException()
    {
        // Arrange
        var nonExistingFile = Path.GetTempPath() + Path.GetRandomFileName();

        // Act
        Action act = () => nonExistingFile.Should().FileExists();

        // Assert
        act
            .Should()
            .Throw<Exception>();
    }

    [Fact]
    public void DirectoryExists_WithExistingDirectory_ReturnsTrue()
    {
        // Arrange
        var tempDirectory = Path.GetTempPath();

        // Act
        Action act = () => tempDirectory.Should().DirectoryExists();

        // Assert
        act
            .Should()
            .NotThrow();
    }

    [Fact]
    public void DirectoryExists_WithNonExistingDirectory_ThrowsException()
    {
        // Arrange
        var nonExistingDirectory = Path.GetTempPath() + Path.GetRandomFileName();

        // Act
        Action act = () => nonExistingDirectory.Should().DirectoryExists();

        // Assert
        act
            .Should()
            .Throw<Exception>();
    }

    [Fact]
    public void FileHasContent_WithMatchingContent_ReturnsTrue()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();

        using var _ = new FileCleanUp(tempFile);

        File.WriteAllText(tempFile, "Test content");

        // Act
        Action act = () => tempFile.Should().FileHasContent("Test content");

        // Assert
        act
            .Should()
            .NotThrow();
    }

    [Fact]
    public void FileHasContent_WithNonMatchingContent_ThrowsException()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();

        using var _ = new FileCleanUp(tempFile);

        File.WriteAllText(tempFile, "Test content");

        // Act
        Action act = () => tempFile.Should().FileHasContent("Different content");

        // Assert
        act
            .Should()
            .Throw<Exception>();
    }

    [Fact]
    public void FileHasContent_WithNonExistingFile_ThrowsException()
    {
        // Arrange
        var nonExistingFile = Path.GetTempPath() + Path.GetRandomFileName();

        // Act
        Action act = () => nonExistingFile.Should().FileHasContent("Test content");

        // Assert
        act
            .Should()
            .Throw<Exception>();
    }
}

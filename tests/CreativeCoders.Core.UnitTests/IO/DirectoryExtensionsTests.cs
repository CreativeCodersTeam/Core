using System.IO.Abstractions;
using CreativeCoders.Core.IO;
using FakeItEasy;
using AwesomeAssertions;
using Xunit;

#nullable enable
namespace CreativeCoders.Core.UnitTests.IO;

public class DirectoryExtensionsTests
{
    [Fact]
    public void EnsureDirectoryExists_ValidPath_CreatesDirectory()
    {
        // Arrange
        var directory = A.Fake<IDirectory>();
        const string path = "/test/path";

        // Act
        directory.EnsureDirectoryExists(path);

        // Assert
        A.CallTo(() => directory.CreateDirectory(path))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void EnsureDirectoryExists_NullOrEmptyPath_DoesNotCreateDirectory(string? path)
    {
        // Arrange
        var directory = A.Fake<IDirectory>();

        // Act
        directory.EnsureDirectoryExists(path);

        // Assert
        A.CallTo(() => directory.CreateDirectory(A<string>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public void EnsureDirectoryForFileNameExists_ValidFilePath_CreatesDirectory()
    {
        // Arrange
        var directory = A.Fake<IDirectory>();
        var fileSystem = A.Fake<IFileSystem>();
        var pathMock = A.Fake<IPath>();

        A.CallTo(() => directory.FileSystem).Returns(fileSystem);
        A.CallTo(() => fileSystem.Path).Returns(pathMock);

        const string filePath = "/test/path/file.txt";
        const string directoryPath = "/test/path";

        A.CallTo(() => pathMock.GetDirectoryName(filePath)).Returns(directoryPath);

        // Act
        directory.EnsureDirectoryForFileNameExists(filePath);

        // Assert
        A.CallTo(() => directory.CreateDirectory(directoryPath))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void EnsureDirectoryForFileNameExists_NullOrEmptyFilePath_DoesNotCreateDirectory(string? filePath)
    {
        // Arrange
        var directory = A.Fake<IDirectory>();

        // Act
        directory.EnsureDirectoryForFileNameExists(filePath);

        // Assert
        A.CallTo(() => directory.CreateDirectory(A<string>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public void EnsureDirectoryForFileNameExists_GetDirectoryNameReturnsNull_DoesNotCreateDirectory()
    {
        // Arrange
        var directory = A.Fake<IDirectory>();
        var fileSystem = A.Fake<IFileSystem>();
        var pathMock = A.Fake<IPath>();

        A.CallTo(() => directory.FileSystem).Returns(fileSystem);
        A.CallTo(() => fileSystem.Path).Returns(pathMock);

        const string filePath = "file.txt";

        A.CallTo(() => pathMock.GetDirectoryName(filePath)).Returns(null);

        // Act
        directory.EnsureDirectoryForFileNameExists(filePath);

        // Assert
        A.CallTo(() => directory.CreateDirectory(A<string>._))
            .MustNotHaveHappened();
    }
}

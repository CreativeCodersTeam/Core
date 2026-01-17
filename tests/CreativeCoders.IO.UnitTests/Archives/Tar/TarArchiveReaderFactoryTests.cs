using System.IO.Abstractions.TestingHelpers;
using CreativeCoders.IO.Archives.Tar;
using AwesomeAssertions;
using Xunit;

namespace CreativeCoders.IO.UnitTests.Archives.Tar;

public class TarArchiveReaderFactoryTests
{
    [Fact]
    public void CreateReader_WithStreamAndNoGZip_ReturnsTarArchiveReader()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var factory = new TarArchiveReaderFactory(fileSystem);
        using var stream = new MemoryStream();

        // Act
        var reader = factory.CreateReader(stream, false);

        // Assert
        reader
            .Should().BeOfType<TarArchiveReader>();
    }

    [Fact]
    public void CreateReader_WithStreamAndGZip_ReturnsTarGzArchiveReader()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var factory = new TarArchiveReaderFactory(fileSystem);
        using var stream = new MemoryStream();

        // Act
        var reader = factory.CreateReader(stream, true);

        // Assert
        reader
            .Should().BeOfType<TarGzArchiveReader>();
    }

    [Fact]
    public void CreateReader_WithTarFile_ReturnsTarArchiveReader()
    {
        // Arrange
        const string fileName = "test.tar";
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(fileName, new MockFileData("test content"));
        var factory = new TarArchiveReaderFactory(fileSystem);

        // Act
        var reader = factory.CreateReader(fileName);

        // Assert
        reader
            .Should().BeOfType<TarArchiveReader>();
    }

    [Fact]
    public void CreateReader_WithTarGzFile_ReturnsTarGzArchiveReader()
    {
        // Arrange
        const string fileName = "test.tar.gz";
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(fileName, new MockFileData("test content"));
        var factory = new TarArchiveReaderFactory(fileSystem);

        // Act
        var reader = factory.CreateReader(fileName);

        // Assert
        reader
            .Should().BeOfType<TarGzArchiveReader>();
    }
}

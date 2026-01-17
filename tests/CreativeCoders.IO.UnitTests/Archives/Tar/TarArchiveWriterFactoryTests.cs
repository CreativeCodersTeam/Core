using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using CreativeCoders.IO.Archives.Tar;
using AwesomeAssertions;
using Xunit;

namespace CreativeCoders.IO.UnitTests.Archives.Tar;

public class TarArchiveWriterFactoryTests
{
    [Fact]
    public void CreateWriter_WithStreamAndNoGZip_ReturnsTarArchiveWriter()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var factory = new TarArchiveWriterFactory(fileSystem);
        using var stream = new MemoryStream();

        // Act
        var writer = factory.CreateWriter(stream, false);

        // Assert
        writer
            .Should().BeOfType<TarArchiveWriter>();
    }

    [Fact]
    public void CreateWriter_WithStreamAndGZip_ReturnsTarArchiveWriter()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var factory = new TarArchiveWriterFactory(fileSystem);
        using var stream = new MemoryStream();

        // Act
        var writer = factory.CreateWriter(stream, true);

        // Assert
        writer
            .Should().BeOfType<TarArchiveWriter>();
    }

    [Fact]
    public void CreateWriter_WithTarFile_ReturnsTarArchiveWriterAndCreatesFile()
    {
        // Arrange
        const string fileName = "/test/test.tar";
        var fileSystem = new MockFileSystem();
        var factory = new TarArchiveWriterFactory(fileSystem);

        // Act
        var writer = factory.CreateWriter(fileName);

        // Assert
        writer
            .Should().BeOfType<TarArchiveWriter>();

        fileSystem.File.Exists(fileName)
            .Should().BeTrue();
    }

    [Fact]
    public void CreateWriter_WithTarGzFile_ReturnsTarArchiveWriterAndCreatesFile()
    {
        // Arrange
        const string fileName = "/test/test.tar.gz";
        var fileSystem = new MockFileSystem();
        var factory = new TarArchiveWriterFactory(fileSystem);

        // Act
        var writer = factory.CreateWriter(fileName);

        // Assert
        writer
            .Should().BeOfType<TarArchiveWriter>();

        fileSystem.File.Exists(fileName)
            .Should().BeTrue();
    }
}

using System.IO.Abstractions.TestingHelpers;
using CreativeCoders.IO.Archives.Zip;
using AwesomeAssertions;
using Xunit;

namespace CreativeCoders.IO.UnitTests.Archives.Zip;

public class ZipArchiveWriterFactoryTests
{
    [Fact]
    public void CreateWriter_WithStream_ReturnsZipArchiveWriter()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var factory = new ZipArchiveWriterFactory(fileSystem);
        using var stream = new MemoryStream();

        // Act
        var writer = factory.CreateWriter(stream);

        // Assert
        writer.Should().BeOfType<ZipArchiveWriter>();
    }

    [Fact]
    public void CreateWriter_WithFileName_ReturnsZipArchiveWriterAndCreatesFile()
    {
        // Arrange
        const string fileName = "/test/test.zip";
        var fileSystem = new MockFileSystem();
        var factory = new ZipArchiveWriterFactory(fileSystem);

        // Act
        var writer = factory.CreateWriter(fileName);

        // Assert
        writer
            .Should().BeOfType<ZipArchiveWriter>();

        fileSystem.File.Exists(fileName)
            .Should().BeTrue();
    }

    [Fact]
    public void CreateWriter_WithFileNameAndOverwriteFalse_ReturnsZipArchiveWriterAndOpensFile()
    {
        // Arrange
        const string fileName = "/test/test.zip";
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(fileName, new MockFileData("existing content"));
        var factory = new ZipArchiveWriterFactory(fileSystem);

        // Act
        var writer = factory.CreateWriter(fileName, overwriteExisting: false);

        // Assert
        writer
            .Should().BeOfType<ZipArchiveWriter>();

        fileSystem.File.Exists(fileName)
            .Should().BeTrue();
    }
}

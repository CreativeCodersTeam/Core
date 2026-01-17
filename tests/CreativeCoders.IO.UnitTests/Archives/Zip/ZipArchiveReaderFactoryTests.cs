using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.IO.Abstractions.TestingHelpers;
using CreativeCoders.IO.Archives.Zip;
using AwesomeAssertions;
using Xunit;

namespace CreativeCoders.IO.UnitTests.Archives.Zip;

public class ZipArchiveReaderFactoryTests
{
    [SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
    [SuppressMessage("csharpsquid", "S6966")]
    private static async Task<byte[]> CreateTestZipArchiveAsync()
    {
        using var stream = new MemoryStream();
        await using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
        {
            var entry = archive.CreateEntry("test.txt");
            await using var entryStream = entry.Open();
            await entryStream.WriteAsync("test content"u8.ToArray());
        }

        return stream.ToArray();
    }

    [Fact]
    [SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
    [SuppressMessage("csharpsquid", "S6966")]
    public async Task CreateReader_WithStream_ReturnsZipArchiveReader()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var factory = new ZipArchiveReaderFactory(fileSystem);
        using var stream = new MemoryStream(await CreateTestZipArchiveAsync());

        // Act
        var reader = factory.CreateReader(stream);

        // Assert
        reader
            .Should().BeOfType<ZipArchiveReader>();
    }

    [Fact]
    [SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
    [SuppressMessage("csharpsquid", "S6966")]
    public async Task CreateReader_WithFileName_ReturnsZipArchiveReader()
    {
        // Arrange
        const string fileName = "test.zip";
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(fileName, new MockFileData(await CreateTestZipArchiveAsync()));
        var factory = new ZipArchiveReaderFactory(fileSystem);

        // Act
        var reader = factory.CreateReader(fileName);

        // Assert
        reader
            .Should().BeOfType<ZipArchiveReader>();
    }

    [Fact]
    public async Task CreateReaderAsync_WithStream_ReturnsZipArchiveReader()
    {
        // Arrange
        var fileSystem = new MockFileSystem();
        var factory = new ZipArchiveReaderFactory(fileSystem);
        using var stream = new MemoryStream(await CreateTestZipArchiveAsync());

        // Act
        var reader = await factory.CreateReaderAsync(stream);

        // Assert
        reader
            .Should().BeOfType<ZipArchiveReader>();
    }

    [Fact]
    public async Task CreateReaderAsync_WithFileName_ReturnsZipArchiveReader()
    {
        // Arrange
        const string fileName = "test.zip";
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(fileName, new MockFileData(await CreateTestZipArchiveAsync()));
        var factory = new ZipArchiveReaderFactory(fileSystem);

        // Act
        var reader = await factory.CreateReaderAsync(fileName);

        // Assert
        reader
            .Should().BeOfType<ZipArchiveReader>();
    }
}

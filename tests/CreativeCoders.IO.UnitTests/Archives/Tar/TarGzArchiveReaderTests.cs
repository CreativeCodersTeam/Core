using System.Diagnostics.CodeAnalysis;
using System.Formats.Tar;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using AwesomeAssertions;
using CreativeCoders.Core.IO;
using CreativeCoders.IO.Archives;
using CreativeCoders.IO.Archives.Tar;
using CreativeCoders.UnitTests;
using Xunit;

namespace CreativeCoders.IO.UnitTests.Archives.Tar;

[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class TarGzArchiveReaderTests
{
    private static async Task<MemoryStream> CreateTestTarGzArchiveAsync()
    {
        var tarStream = new MemoryStream();
        await using (var writer = new TarWriter(tarStream, TarEntryFormat.Pax, true))
        {
            var entry1 = new PaxTarEntry(TarEntryType.RegularFile, "file1.txt")
            {
                DataStream = new MemoryStream("Content 1"u8.ToArray())
            };
            await writer.WriteEntryAsync(entry1);

            var entry2 = new PaxTarEntry(TarEntryType.RegularFile, "dir/file2.txt")
            {
                DataStream = new MemoryStream("Content 2"u8.ToArray())
            };
            await writer.WriteEntryAsync(entry2);
        }

        tarStream.Position = 0;
        var gzStream = new MemoryStream();
        await using (var compressionStream = new GZipStream(gzStream, CompressionMode.Compress, true))
        {
            await tarStream.CopyToAsync(compressionStream);
        }

        gzStream.Position = 0;
        return gzStream;
    }

    [Fact]
    public async Task GetEntriesAsync_ArchiveWithEntries_ReturnsAllEntries()
    {
        // Arrange
        using var archiveStream = await CreateTestTarGzArchiveAsync();
        await using var reader = new TarGzArchiveReader(archiveStream, new MockFileSystem());

        // Act
        var entries = await reader.GetEntriesAsync().ToListAsync();

        // Assert
        entries
            .Should().HaveCount(2);

        entries.Select(x => x.FullName)
            .Should().BeEquivalentTo("file1.txt", "dir/file2.txt");
    }

    [Fact]
    public async Task GetEntries_ArchiveWithEntries_ReturnsAllEntries()
    {
        // Arrange
        using var archiveStream = await CreateTestTarGzArchiveAsync();
        await using var reader = new TarGzArchiveReader(archiveStream, new MockFileSystem());

        // Act
        var entries = reader.GetEntries().ToList();

        // Assert
        entries
            .Should().HaveCount(2);

        entries.Select(x => x.FullName)
            .Should().BeEquivalentTo("file1.txt", "dir/file2.txt");
    }

    [Fact]
    public async Task OpenEntryStreamAsync_ExistingEntry_ReturnsCorrectStream()
    {
        // Arrange
        using var archiveStream = await CreateTestTarGzArchiveAsync();
        await using var reader = new TarGzArchiveReader(archiveStream, new MockFileSystem());
        var entry = new ArchiveEntry("file1.txt");

        // Act
        await using var entryStream = await reader.OpenEntryStreamAsync(entry);
        using var streamReader = new StreamReader(entryStream);
        var content = await streamReader.ReadToEndAsync();

        // Assert
        content
            .Should().Be("Content 1");
    }

    [Fact]
    public async Task OpenEntryStreamAsync_WithCopyData_ReturnsCopyOfStream()
    {
        // Arrange
        using var archiveStream = await CreateTestTarGzArchiveAsync();
        await using var reader = new TarGzArchiveReader(archiveStream, new MockFileSystem());
        var entry = new ArchiveEntry("dir/file2.txt");

        // Act
        await using var entryStream = await reader.OpenEntryStreamAsync(entry, true);
        using var streamReader = new StreamReader(entryStream);
        var content = await streamReader.ReadToEndAsync();

        // Assert
        content
            .Should().Be("Content 2");

        entryStream
            .Should().BeOfType<MemoryStream>();
    }

    [Fact]
    public async Task ExtractFileAsync_ExistingEntry_ExtractsFileToFileSystem()
    {
        // Arrange
        using var archiveStream = await CreateTestTarGzArchiveAsync();
        await using var reader = new TarGzArchiveReader(archiveStream, new MockFileSystem());
        var entry = new ArchiveEntry("file1.txt");
        var tempFile = Path.GetTempFileName();
        using var fileCleanup = new FileCleanUp(tempFile);

        // Act
        await reader.ExtractFileAsync(entry, tempFile);

        // Assert
        File.Exists(tempFile)
            .Should().BeTrue();

        (await File.ReadAllTextAsync(tempFile))
            .Should().Be("Content 1");
    }

    [Fact]
    public async Task ExtractFileWithPathAsync_ExistingEntry_ExtractsFileWithRelativePath()
    {
        // Arrange
        var fakeFileSystem = new FileSystem();
        using var archiveStream = await CreateTestTarGzArchiveAsync();
        await using var reader = new TarGzArchiveReader(archiveStream, fakeFileSystem);
        var entry = new ArchiveEntry("dir/file2.txt");
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        using var dirCleanup = new DirectoryCleanUp(tempDir);

        // Act
        var resultPath = await reader.ExtractFileWithPathAsync(entry, tempDir);

        // Assert
        fakeFileSystem.File.Exists(resultPath)
            .Should().BeTrue();

        resultPath
            .Should().Be(fakeFileSystem.Path.Combine(tempDir, "dir/file2.txt"));

        (await fakeFileSystem.File.ReadAllTextAsync(resultPath))
            .Should().Be("Content 2");
    }

    [Fact]
    public async Task ExtractAllAsync_ArchiveWithEntries_ExtractsAllFiles()
    {
        // Arrange
        var fakeFileSystem = new FileSystem();
        using var archiveStream = await CreateTestTarGzArchiveAsync();
        await using var reader = new TarGzArchiveReader(archiveStream, fakeFileSystem);
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        using var dirCleanup = new DirectoryCleanUp(tempDir);

        // Act
        await reader.ExtractAllAsync(tempDir);

        // Assert
        fakeFileSystem.File.Exists(fakeFileSystem.Path.Combine(tempDir, "file1.txt"))
            .Should().BeTrue();
        fakeFileSystem.File.Exists(fakeFileSystem.Path.Combine(tempDir, "dir/file2.txt"))
            .Should().BeTrue();

        (await fakeFileSystem.File.ReadAllTextAsync(fakeFileSystem.Path.Combine(tempDir, "file1.txt")))
            .Should().Be("Content 1");

        (await fakeFileSystem.File.ReadAllTextAsync(fakeFileSystem.Path.Combine(tempDir, "dir/file2.txt")))
            .Should().Be("Content 2");
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task OpenEntryStreamAsync_NonExistingEntry_ThrowsFileNotFoundException()
    {
        // Arrange
        using var archiveStream = await CreateTestTarGzArchiveAsync();
        await using var reader = new TarGzArchiveReader(archiveStream, new MockFileSystem());
        var entry = new ArchiveEntry("non_existing.txt");

        // Act
        var act = () => reader.OpenEntryStreamAsync(entry);

        // Assert
        await act
            .Should().ThrowAsync<FileNotFoundException>();
    }
}

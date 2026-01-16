using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Text;
using AwesomeAssertions;
using CreativeCoders.Core.IO;
using CreativeCoders.IO.Archives;
using CreativeCoders.IO.Archives.Zip;
using CreativeCoders.UnitTests;

namespace CreativeCoders.IO.UnitTests.Archives.Zip;

[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class ZipArchiveReaderTests
{
    private static async Task<MemoryStream> CreateTestZipArchiveAsync()
    {
        var stream = new MemoryStream();
        await using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
        {
            var entry1 = archive.CreateEntry("file1.txt");
            await using (var entryStream = await entry1.OpenAsync())
            {
                await entryStream.WriteAsync("Content 1"u8.ToArray());
            }

            var entry2 = archive.CreateEntry("dir/file2.txt");
            await using (var entryStream = await entry2.OpenAsync())
            {
                await entryStream.WriteAsync("Content 2"u8.ToArray());
            }
        }

        stream.Position = 0;
        return stream;
    }

    [Fact]
    [SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
    [SuppressMessage("csharpsquid", "S6966")]
    public async Task GetEntriesAsync_ArchiveWithEntries_ReturnsAllEntries()
    {
        // Arrange
        using var archiveStream = await CreateTestZipArchiveAsync();
        await using var reader = ZipArchiveReader.Create(archiveStream);

        // Act
        var entries = await reader.GetEntriesAsync().ToArrayAsync();

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
        using var archiveStream = await CreateTestZipArchiveAsync();
        await using var reader = await ZipArchiveReader.CreateAsync(archiveStream);

        // Act
        var entries = reader.GetEntries().ToArray();

        // Assert
        entries
            .Should().HaveCount(2);

        entries.Select(x => x.FullName)
            .Should().BeEquivalentTo("file1.txt", "dir/file2.txt");
    }

    [Fact]
    [SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
    [SuppressMessage("csharpsquid", "S6966")]
    public async Task OpenEntryStreamAsync_ExistingEntry_ReturnsCorrectStream()
    {
        // Arrange
        using var archiveStream = await CreateTestZipArchiveAsync();
        await using var reader = ZipArchiveReader.Create(archiveStream);
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
    public async Task ExtractFileAsync_ExistingEntry_ExtractsFileToFileSystem()
    {
        // Arrange
        using var archiveStream = await CreateTestZipArchiveAsync();
        await using var reader = await ZipArchiveReader.CreateAsync(archiveStream);
        var entry = new ArchiveEntry("file1.txt");
        var tempFile = Path.GetTempFileName();

        using var fileCleanup = new FileCleanUp(tempFile);

        // Act
        await reader.ExtractFileAsync(entry, tempFile);

        // Assert
        var content = await File.ReadAllTextAsync(tempFile);

        content
            .Should().Be("Content 1");
    }

    [Fact]
    [SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
    [SuppressMessage("csharpsquid", "S6966")]
    public async Task ExtractFileWithPathAsync_ExistingEntry_ExtractsFileWithRelativePath()
    {
        // Arrange
        using var archiveStream = await CreateTestZipArchiveAsync();
        await using var reader = ZipArchiveReader.Create(archiveStream);
        var entry = new ArchiveEntry("dir/file2.txt");
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        using var dirCleanup = new DirectoryCleanUp(tempDir);

        // Act
        var extractedFile = await reader.ExtractFileWithPathAsync(entry, tempDir);

        // Assert
        extractedFile
            .Should().Be(Path.Combine(tempDir, "dir", "file2.txt"));

        File.Exists(extractedFile)
            .Should().BeTrue();

        var content = await File.ReadAllTextAsync(extractedFile);

        content
            .Should().Be("Content 2");
    }

    [Fact]
    [SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
    [SuppressMessage("csharpsquid", "S6966")]
    public async Task ExtractAllAsync_ArchiveWithEntries_ExtractsAllFiles()
    {
        // Arrange
        using var archiveStream = await CreateTestZipArchiveAsync();
        await using var reader = ZipArchiveReader.Create(archiveStream);
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        using var dirCleanup = new DirectoryCleanUp(tempDir);

        // Act
        await reader.ExtractAllAsync(tempDir);

        // Assert
        var file1 = Path.Combine(tempDir, "file1.txt");
        var file2 = Path.Combine(tempDir, "dir", "file2.txt");

        File.Exists(file1)
            .Should().BeTrue();

        File.Exists(file2)
            .Should().BeTrue();

        (await File.ReadAllTextAsync(file1))
            .Should().Be("Content 1");

        (await File.ReadAllTextAsync(file2))
            .Should().Be("Content 2");
    }

    [Fact]
    [SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
    [SuppressMessage("csharpsquid", "S6966")]
    public async Task OpenEntryStreamAsync_NonExistingEntry_ThrowsFileNotFoundException()
    {
        // Arrange
        using var archiveStream = await CreateTestZipArchiveAsync();
        await using var reader = ZipArchiveReader.Create(archiveStream);
        var entry = new ArchiveEntry("non_existing.txt");

        // Act & Assert
        await reader.Awaiting(x => x.OpenEntryStreamAsync(entry))
            .Should().ThrowAsync<FileNotFoundException>();
    }
}

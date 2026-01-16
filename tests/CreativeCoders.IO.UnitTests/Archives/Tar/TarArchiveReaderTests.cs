using System.Diagnostics.CodeAnalysis;
using System.Formats.Tar;
using System.IO.Abstractions;
using System.Text;
using AwesomeAssertions;
using CreativeCoders.IO.Archives;
using CreativeCoders.IO.Archives.Tar;
using CreativeCoders.UnitTests;
using FakeItEasy;

namespace CreativeCoders.IO.UnitTests.Archives.Tar;

[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
[Collection("FileSys")]
public class TarArchiveReaderTests
{
    private readonly MockFileSystemEx _fileSystem = new MockFileSystemEx();

    private static async Task<MemoryStream> CreateTestTarArchiveAsync()
    {
        var stream = new MemoryStream();
        await using (var writer = new TarWriter(stream, TarEntryFormat.Pax, true))
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

        stream.Position = 0;
        return stream;
    }

    [Fact]
    public async Task GetEntriesAsync_ArchiveWithEntries_ReturnsAllEntries()
    {
        // Arrange
        using var archiveStream = await CreateTestTarArchiveAsync();
        await using var reader = new TarArchiveReader(archiveStream, _fileSystem);

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
        using var archiveStream = await CreateTestTarArchiveAsync();
        await using var reader = new TarArchiveReader(archiveStream, _fileSystem);

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
        using var archiveStream = await CreateTestTarArchiveAsync();
        await using var reader = new TarArchiveReader(archiveStream, _fileSystem);
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
        using var archiveStream = await CreateTestTarArchiveAsync();
        await using var reader = new TarArchiveReader(archiveStream, _fileSystem);
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
        using var archiveStream = await CreateTestTarArchiveAsync();
        await using var reader = new TarArchiveReader(archiveStream, _fileSystem);
        var entry = new ArchiveEntry("file1.txt");
        const string outputFilePath = "/output/file1.txt";

        // Act
        await reader.ExtractFileAsync(entry, outputFilePath);

        // Assert
        _fileSystem.File.Exists(outputFilePath)
            .Should().BeTrue();

        (await _fileSystem.File.ReadAllTextAsync(outputFilePath))
            .Should().Be("Content 1");
    }

    [Fact]
    public async Task ExtractFileWithPathAsync_ExistingEntry_ExtractsFileWithRelativePath()
    {
        // Arrange
        using var archiveStream = await CreateTestTarArchiveAsync();
        await using var reader = new TarArchiveReader(archiveStream, _fileSystem);
        var entry = new ArchiveEntry("dir/file2.txt");
        const string outputBaseDirectory = "/output_base";

        // Act
        var resultPath = await reader.ExtractFileWithPathAsync(entry, outputBaseDirectory);

        // Assert
        _fileSystem.File.Exists(resultPath)
            .Should().BeTrue();

        resultPath
            .Should().Be(_fileSystem.Path.Combine(outputBaseDirectory, "dir/file2.txt"));

        (await _fileSystem.File.ReadAllTextAsync(resultPath))
            .Should().Be("Content 2");
    }

    [Fact]
    public async Task ExtractAllAsync_ArchiveWithEntries_ExtractsAllFiles()
    {
        // Arrange
        using var archiveStream = await CreateTestTarArchiveAsync();
        await using var reader = new TarArchiveReader(archiveStream, _fileSystem);
        const string outputBaseDirectory = "/extract_all";

        // Act
        await reader.ExtractAllAsync(outputBaseDirectory);

        // Assert
        _fileSystem.File.Exists(_fileSystem.Path.Combine(outputBaseDirectory, "file1.txt"))
            .Should().BeTrue();
        _fileSystem.File.Exists(_fileSystem.Path.Combine(outputBaseDirectory, "dir/file2.txt"))
            .Should().BeTrue();

        (await _fileSystem.File.ReadAllTextAsync(_fileSystem.Path.Combine(outputBaseDirectory, "file1.txt")))
            .Should().Be("Content 1");

        (await _fileSystem.File.ReadAllTextAsync(_fileSystem.Path.Combine(outputBaseDirectory,
                "dir/file2.txt")))
            .Should().Be("Content 2");
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task OpenEntryStreamAsync_NonExistingEntry_ThrowsFileNotFoundException()
    {
        // Arrange
        using var archiveStream = await CreateTestTarArchiveAsync();
        await using var reader = new TarArchiveReader(archiveStream, _fileSystem);
        var entry = new ArchiveEntry("non_existing.txt");

        // Act
        var act = () => reader.OpenEntryStreamAsync(entry);

        // Assert
        await act
            .Should().ThrowAsync<FileNotFoundException>();
    }
}

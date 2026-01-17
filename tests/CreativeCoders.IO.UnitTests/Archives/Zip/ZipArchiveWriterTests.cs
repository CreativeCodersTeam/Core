using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Text;
using AwesomeAssertions;
using CreativeCoders.Core.IO;
using CreativeCoders.IO.Archives.Zip;

namespace CreativeCoders.IO.UnitTests.Archives.Zip;

[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
[Collection("FileSys")]
public class ZipArchiveWriterTests
{
    [Fact]
    public async Task AddFileAsync_FromFileName_FileIsAddedToArchive()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        const string fileContent = "Test content";
        await File.WriteAllTextAsync(tempFile, fileContent);

        using var fileCleanup = new FileCleanUp(tempFile);

        const string fileNameInArchive = "test_dir/test_file.txt";
        using var outputStream = new MemoryStream();

        // Act
        await using (var writer = new ZipArchiveWriter(outputStream, CompressionLevel.Optimal, true))
        {
            await writer.AddFileAsync(tempFile, fileNameInArchive);
        }

        // Assert
        outputStream.Position = 0;
        await using var archive = new ZipArchive(outputStream, ZipArchiveMode.Read);
        var entry = archive.GetEntry("test_dir/test_file.txt");

        entry
            .Should().NotBeNull();

        entry!.FullName
            .Should().Be("test_dir/test_file.txt");

        await using var entryStream = await entry.OpenAsync();
        using var readerStream = new StreamReader(entryStream, Encoding.UTF8);
        var content = await readerStream.ReadToEndAsync();

        content
            .Should().Be(fileContent);
    }

    [Fact]
    public async Task AddFileAsync_FromStream_FileIsAddedToArchive()
    {
        // Arrange
        const string fileContent = "Stream content";
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
        const string fileNameInArchive = "stream_file.txt";
        using var outputStream = new MemoryStream();

        // Act
        await using (var writer = new ZipArchiveWriter(outputStream, CompressionLevel.Optimal, true))
        {
            await writer.AddFileAsync(inputStream, fileNameInArchive);
        }

        // Assert
        outputStream.Position = 0;
        await using var archive = new ZipArchive(outputStream, ZipArchiveMode.Read);
        var entry = archive.GetEntry(fileNameInArchive);

        entry
            .Should().NotBeNull();

        entry!.FullName
            .Should().Be(fileNameInArchive);

        await using var entryStream = await entry.OpenAsync();
        using var readerStream = new StreamReader(entryStream, Encoding.UTF8);
        var content = await readerStream.ReadToEndAsync();

        content
            .Should().Be(fileContent);
    }

    [Fact]
    public async Task AddFileAsync_WithCompressionLevel_FileIsAddedWithCorrectCompression()
    {
        // Arrange
        const string fileContent = "Compressed content";
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
        const string fileNameInArchive = "compressed_file.txt";
        using var outputStream = new MemoryStream();

        // Act
        await using (var writer = new ZipArchiveWriter(outputStream, CompressionLevel.NoCompression, true))
        {
            await writer.AddFileAsync(inputStream, fileNameInArchive, CompressionLevel.SmallestSize);
        }

        // Assert
        outputStream.Position = 0;
        await using var archive = new ZipArchive(outputStream, ZipArchiveMode.Read);
        var entry = archive.GetEntry(fileNameInArchive);

        entry
            .Should().NotBeNull();

        await using var entryStream = await entry!.OpenAsync();
        using var readerStream = new StreamReader(entryStream, Encoding.UTF8);
        var content = await readerStream.ReadToEndAsync();

        content
            .Should().Be(fileContent);
    }

    [Fact]
    public async Task AddFromDirectoryAsync_DirectoryIsAddedToArchive()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), "ZipArchiveWriterTests_" + Guid.NewGuid());
        Directory.CreateDirectory(tempDir);
        var subDir = Path.Combine(tempDir, "SubDir");
        Directory.CreateDirectory(subDir);

        await File.WriteAllTextAsync(Path.Combine(tempDir, "file1.txt"), "Content 1");
        await File.WriteAllTextAsync(Path.Combine(subDir, "file2.txt"), "Content 2");

        using var dirCleanup = new DirectoryCleanUp(tempDir);

        using var outputStream = new MemoryStream();

        // Act
        await using (var writer = new ZipArchiveWriter(outputStream, CompressionLevel.Optimal, true))
        {
            await writer.AddFromDirectoryAsync(tempDir, tempDir);
        }

        // Assert
        outputStream.Position = 0;
        await using var archive = new ZipArchive(outputStream, ZipArchiveMode.Read);

        archive.Entries.Count
            .Should().Be(2);

        var entry1 = archive.GetEntry("file1.txt");
        entry1
            .Should().NotBeNull();

        var entry2 = archive.GetEntry("SubDir/file2.txt") ??
                     archive.GetEntry("SubDir" + Path.DirectorySeparatorChar + "file2.txt");

        entry2
            .Should().NotBeNull();
    }

    [Fact]
    public async Task DisposeAsync_ArchiveIsCorrectlyFinalized()
    {
        // Arrange
        using var outputStream = new MemoryStream();
        const string fileNameInArchive = "finalized_file.txt";

        // Act
        await using (var writer = new ZipArchiveWriter(outputStream, CompressionLevel.Optimal, true))
        {
            using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("Finalized content"));
            await writer.AddFileAsync(inputStream, fileNameInArchive);
        }

        // Assert
        outputStream.Position = 0;
        await using var archive = new ZipArchive(outputStream, ZipArchiveMode.Read);

        archive.GetEntry(fileNameInArchive)
            .Should().NotBeNull();
    }
}

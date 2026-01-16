using System.Diagnostics.CodeAnalysis;
using System.Formats.Tar;
using System.Text;
using AwesomeAssertions;
using CreativeCoders.Core.IO;
using CreativeCoders.IO.Archives.Tar;

namespace CreativeCoders.IO.UnitTests.Archives.Tar;

[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
[Collection("FileSys")]
public class TarArchiveWriterTests
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
        await using (var writer = new TarArchiveWriter(outputStream, true))
        {
            await writer.AddFileAsync(tempFile, fileNameInArchive);
        }

        // Assert
        outputStream.Position = 0;
        await using var reader = new TarReader(outputStream);
        var entry = await reader.GetNextEntryAsync();

        entry
            .Should().NotBeNull();

        entry!.Name
            .Should().Be("test_dir/test_file.txt");

        entry.EntryType
            .Should().Be(TarEntryType.RegularFile);

        await using var entryStream = entry.DataStream!;
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
        await using (var writer = new TarArchiveWriter(outputStream, true))
        {
            await writer.AddFileAsync(inputStream, fileNameInArchive);
        }

        // Assert
        outputStream.Position = 0;
        await using var reader = new TarReader(outputStream);
        var entry = await reader.GetNextEntryAsync();

        entry
            .Should().NotBeNull();

        entry!.Name
            .Should().Be(fileNameInArchive);

        await using var entryStream = entry.DataStream!;
        using var readerStream = new StreamReader(entryStream, Encoding.UTF8);
        var content = await readerStream.ReadToEndAsync();

        content
            .Should().Be(fileContent);
    }

    [Fact]
    public async Task AddFileAsync_WithConfigureEntry_MetadataIsSet()
    {
        // Arrange
        const string fileContent = "Content with metadata";
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
        const string fileNameInArchive = "metadata_file.txt";
        using var outputStream = new MemoryStream();

        const string userName = "testuser";
        const string groupName = "testgroup";
        const int userId = 1000;
        const int groupId = 1000;
        const UnixFileMode fileMode = UnixFileMode.UserRead | UnixFileMode.UserWrite;

        // Act
        await using (var writer = new TarArchiveWriter(outputStream, true))
        {
            await writer.AddFileAsync(inputStream, fileNameInArchive, x =>
            {
                x.UserName = userName;
                x.GroupName = groupName;
                x.UserId = userId;
                x.GroupId = groupId;
                x.FileMode = fileMode;
            });
        }

        // Assert
        outputStream.Position = 0;
        await using var reader = new TarReader(outputStream);
        var entry = await reader.GetNextEntryAsync() as PaxTarEntry;

        entry
            .Should().NotBeNull();

        entry!.UserName
            .Should().Be(userName);

        entry.GroupName
            .Should().Be(groupName);

        entry.Uid
            .Should().Be(userId);

        entry.Gid
            .Should().Be(groupId);

        entry.Mode
            .Should().Be(fileMode);
    }

    [Fact]
    public async Task AddFileAsync_WithTarEntryInfo_MetadataIsSet()
    {
        // Arrange
        const string fileContent = "Content with TarEntryInfo";
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
        const string fileNameInArchive = "info_file.txt";
        using var outputStream = new MemoryStream();

        var entryInfo = new TarEntryInfo
        {
            UserName = "userinfo",
            GroupName = "groupinfo",
            UserId = 123,
            GroupId = 456,
            FileMode = UnixFileMode.UserRead
        };

        // Act
        await using (var writer = new TarArchiveWriter(outputStream, true))
        {
            await writer.AddFileAsync(inputStream, fileNameInArchive, entryInfo);
        }

        // Assert
        outputStream.Position = 0;
        await using var reader = new TarReader(outputStream);
        var entry = await reader.GetNextEntryAsync() as PaxTarEntry;

        entry
            .Should().NotBeNull();

        entry!.UserName
            .Should().Be(entryInfo.UserName);

        entry.GroupName
            .Should().Be(entryInfo.GroupName);

        entry.Uid
            .Should().Be(entryInfo.UserId);

        entry.Gid
            .Should().Be(entryInfo.GroupId);

        entry.Mode
            .Should().Be(entryInfo.FileMode);
    }

    [Fact]
    public async Task AddFromDirectoryAsync_DirectoryIsAddedToArchive()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        var subDir = Path.Combine(tempDir, "subdir");
        Directory.CreateDirectory(subDir);

        using var directoryCleanup = new DirectoryCleanUp(tempDir);

        var file1 = Path.Combine(tempDir, "file1.txt");
        var file2 = Path.Combine(subDir, "file2.txt");

        await File.WriteAllTextAsync(file1, "content1");
        await File.WriteAllTextAsync(file2, "content2");

        using var outputStream = new MemoryStream();

        // Act
        await using (var writer = new TarArchiveWriter(outputStream, true))
        {
            await writer.AddFromDirectoryAsync(tempDir, tempDir);
        }

        // Assert
        outputStream.Position = 0;
        await using var reader = new TarReader(outputStream);
        var entries = new List<TarEntry>();
        TarEntry? entry;
        while ((entry = await reader.GetNextEntryAsync()) != null)
        {
            entries.Add(entry);
        }

        entries
            .Should().HaveCount(2);

        entries
            .Should().Contain(x => x.Name == "file1.txt");

        entries
            .Should().Contain(x => x.Name == "subdir/file2.txt");
    }

    [Fact]
    public async Task DisposeAsync_ArchiveIsCorrectlyFinalized()
    {
        // Arrange
        using var outputStream = new MemoryStream();

        // Act
        await using (var writer = new TarArchiveWriter(outputStream, true))
        {
            await writer.AddFileAsync(new MemoryStream(Encoding.UTF8.GetBytes("test")), "test.txt");
        }

        // Assert
        outputStream.Position = 0;
        await using var reader = new TarReader(outputStream);
        var entry = await reader.GetNextEntryAsync();

        entry
            .Should().NotBeNull();

        (await reader.GetNextEntryAsync())
            .Should().BeNull(); // End of archive reached correctly
    }
}

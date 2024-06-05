using System.IO.Compression;
using CreativeCoders.Core.IO;
using CreativeCoders.IO.Archives;
using CreativeCoders.UnitTests;
using FluentAssertions;

namespace CreativeCoders.IO.UnitTests.Archives;

[Collection("FileSys")]
public class ZipArchiveWriterIntegrationTests
{
    [Fact]
    public void AddFromFile_WithFileNameAndEntryName_AddsFileToArchive()
    {
        // Arrange
        var file = FileSys.FileInfo.New(@"D:\source\bak\TarFileOwnerInfo.cs");
        var zipFile = @"c:\temp\test.zip"; // Path.GetTempFileName();
        var zipArchiveFactory = new ZipArchiveFactory();
        using var zipArchiveWriter = zipArchiveFactory.CreateArchiveWriter(zipFile);

        // Act
        zipArchiveWriter.AddFromFile(file.FullName, file.Name);

        // Assert
    }

    [Fact]
    public void AddFromDirectory_WithDirectoryPathAndEntryPathName_AddsDirectoryToArchive()
    {
        // Arrange
        var directory = FileSys.DirectoryInfo.New(@"D:\source\bak");
        var zipFile = @"c:\temp\test.zip"; // Path.GetTempFileName();
        var zipArchiveFactory = new ZipArchiveFactory();
        using var zipArchiveWriter = zipArchiveFactory.CreateArchiveWriter(zipFile);

        // Act
        zipArchiveWriter.AddFromDirectory(directory.FullName, directory.Name);

        // Assert
    }

    [Fact]
    public void AddFromDirectory_WithDirectoryAndSubdirectories_AddsDirectoryToArchiveAndExtractsCorrectly()
    {
        // Arrange
        var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        using var _ = new DirectoryCleanUp(tempDirectory);

        Directory.CreateDirectory(tempDirectory);
        Directory.CreateDirectory(Path.Combine(tempDirectory, "subdir1"));
        Directory.CreateDirectory(Path.Combine(tempDirectory, "subdir2"));
        File.WriteAllText(Path.Combine(tempDirectory, "file1.txt"), "Test content 1");
        File.WriteAllText(Path.Combine(tempDirectory, "subdir1", "file2.txt"), "Test content 2");
        File.WriteAllText(Path.Combine(tempDirectory, "subdir2", "file3.txt"), "Test content 3");

        var zipFile = Path.GetTempFileName();
        File.Delete(zipFile); // GetTempFileName creates a file, but we need a name for a new file

        var zipArchiveFactory = new ZipArchiveFactory();
        using (var zipArchiveWriter = zipArchiveFactory.CreateArchiveWriter(zipFile))
        {
            // Act
            zipArchiveWriter.AddFromDirectory(tempDirectory, Path.GetFileName(tempDirectory), true);
        }

        // Extract
        var extractPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(extractPath);
        ZipFile.ExtractToDirectory(zipFile, extractPath);

        // Assert
        Path.Combine(extractPath, Path.GetFileName(tempDirectory), "subdir1")
            .Should()
            .DirectoryExists();

        Assert.True(Directory.Exists(Path.Combine(extractPath, Path.GetFileName(tempDirectory), "subdir1")));
        Assert.True(Directory.Exists(Path.Combine(extractPath, Path.GetFileName(tempDirectory), "subdir2")));
        Assert.True(File.Exists(Path.Combine(extractPath, Path.GetFileName(tempDirectory), "file1.txt")));
        Assert.True(File.Exists(Path.Combine(extractPath, Path.GetFileName(tempDirectory), "subdir1",
            "file2.txt")));
        Assert.True(File.Exists(Path.Combine(extractPath, Path.GetFileName(tempDirectory), "subdir2",
            "file3.txt")));
        Assert.Equal("Test content 1",
            File.ReadAllText(Path.Combine(extractPath, Path.GetFileName(tempDirectory), "file1.txt")));
        Assert.Equal("Test content 2",
            File.ReadAllText(Path.Combine(extractPath, Path.GetFileName(tempDirectory), "subdir1",
                "file2.txt")));
        Assert.Equal("Test content 3",
            File.ReadAllText(Path.Combine(extractPath, Path.GetFileName(tempDirectory), "subdir2",
                "file3.txt")));
    }
}

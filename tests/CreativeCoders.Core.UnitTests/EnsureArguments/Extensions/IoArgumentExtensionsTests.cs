using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using CreativeCoders.Core.IO;
using CreativeCoders.UnitTests;
using AwesomeAssertions;
using Xunit;

#nullable enable
namespace CreativeCoders.Core.UnitTests.EnsureArguments.Extensions;

[Collection("FileSys")]
public class IoArgumentExtensionsTests
{
    [Fact]
    public void FileExists_ExistingFileName_ReturnsArgument()
    {
        var fileName = Path.GetTempFileName();

        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>
            {
                { fileName, new MockFileData(string.Empty) }
            },
            Path.GetTempPath());

        mockFileSystem.Install();

        // Act
        var argument = Ensure.Argument(fileName).FileExists();

        // Assert
        argument
            .Should()
            .BeOfType<ArgumentNotNull<string>>();

        argument.Value
            .Should()
            .Be(fileName);
    }

    [Fact]
    public void FileExists_NotExistingFileName_ThrowsException()
    {
        var fileName = Path.GetTempFileName();

        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>(),
            Path.GetTempPath());

        mockFileSystem.Install();

        // Act
        Action act = () => Ensure.Argument(fileName).FileExists();

        // Assert
        act
            .Should()
            .Throw<FileNotFoundException>();
    }

    [Fact]
    public void FileExists_ArgNotNullExistingFileName_ReturnsArgument()
    {
        var fileName = Path.GetTempFileName();

        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>
            {
                { fileName, new MockFileData(string.Empty) }
            },
            Path.GetTempPath());

        mockFileSystem.Install();

        // Act
        var argument = Ensure.Argument(fileName).NotNull().FileExists();

        // Assert
        argument
            .Should()
            .BeOfType<ArgumentNotNull<string>>();

        argument.Value
            .Should()
            .Be(fileName);
    }

    [Fact]
    public void FileExists_ArgNotNullNotExistingFileName_ThrowsException()
    {
        var fileName = Path.GetTempFileName();

        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>(),
            Path.GetTempPath());

        mockFileSystem.Install();

        // Act
        Action act = () => Ensure.Argument(fileName).NotNull().FileExists();

        // Assert
        act
            .Should()
            .Throw<FileNotFoundException>();
    }

    [Fact]
    public void FileExists_NullFileName_ThrowsException()
    {
        const string? fileName = null;

        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>(),
            Path.GetTempPath());

        mockFileSystem.Install();

        // Act
        Action act = () => Ensure.Argument(fileName).FileExists();

        // Assert
        act
            .Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void DirectoryExists_ExistingDirectoryName_ReturnsArgument()
    {
        var fileName = Path.GetTempFileName();

        var directoryName = FileSys.Path.GetDirectoryName(fileName);

        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>
            {
                { fileName, new MockFileData(string.Empty) }
            },
            Path.GetTempPath());

        mockFileSystem.Install();

        // Act
        var argument = Ensure.Argument(directoryName).DirectoryExists();

        // Assert
        argument
            .Should()
            .BeOfType<ArgumentNotNull<string>>();

        argument.Value
            .Should()
            .Be(directoryName);
    }

    [Fact]
    public void DirectoryExists_NotExistingDirectoryName_ThrowsException()
    {
        var directoryName = Path.Combine(Path.GetTempPath(), "sub_dir");

        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>(),
            Path.GetTempPath());

        mockFileSystem.Install();

        // Act
        Action act = () => Ensure.Argument(directoryName).DirectoryExists();

        // Assert
        act
            .Should()
            .Throw<DirectoryNotFoundException>();
    }

    [Fact]
    public void DirectoryExists_ArgNotNullExistingDirectoryName_ReturnsArgument()
    {
        var fileName = Path.GetTempFileName();
        var directoryName = FileSys.Path.GetDirectoryName(fileName);

        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>
            {
                { fileName, new MockFileData(string.Empty) }
            },
            Path.GetTempPath());

        mockFileSystem.Install();

        // Act
        var argument = Ensure.Argument(directoryName).NotNull().DirectoryExists();

        // Assert
        argument
            .Should()
            .BeOfType<ArgumentNotNull<string>>();

        argument.Value
            .Should()
            .Be(directoryName);
    }

    [Fact]
    public void DirectoryExists_ArgNotNullNotExistingDirectoryName_ThrowsException()
    {
        var directoryName = Path.Combine(Path.GetTempPath(), "sub_dir");

        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>(),
            Path.GetTempPath());

        mockFileSystem.Install();

        // Act
        Action act = () => Ensure.Argument(directoryName).NotNull().DirectoryExists();

        // Assert
        act
            .Should()
            .Throw<DirectoryNotFoundException>();
    }

    [Fact]
    public void DirectoryExists_NullDirectoryName_ThrowsException()
    {
        const string? directoryName = null;

        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>(),
            Path.GetTempPath());

        mockFileSystem.Install();

        // Act
        Action act = () => Ensure.Argument(directoryName).DirectoryExists();

        // Assert
        act
            .Should()
            .Throw<ArgumentNullException>();
    }
}

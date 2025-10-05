using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using CreativeCoders.Core.IO;
using CreativeCoders.UnitTests;
using AwesomeAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.IO;

[Collection("FileSys")]
public class FileCleanUpTests
{
    [Fact]
    public void Dispose_FileNotExistsNoThrow_NotThrowsException()
    {
        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>(),
            Path.GetTempPath());

        mockFileSystem.Install();

        var fileName = FileSys.Path.Combine(FileSys.Path.GetTempPath(), FileSys.Path.GetRandomFileName());

        // Arrange
        var fcu = new FileCleanUp(fileName);

        // Act
        var act = () => fcu.Dispose();

        // Assert
        act
            .Should()
            .NotThrow();
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public void Dispose_FileNotExistsThrow_ThrowsException()
    {
        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>(),
            Path.GetTempPath());

        mockFileSystem.Install();

        var fileName = FileSys.Path.Combine(FileSys.Path.GetTempPath(), FileSys.Path.GetRandomFileName());

        FileSys.Directory.CreateDirectory(FileSys.Path.GetDirectoryName(fileName)!);

        // Arrange
        var fcu = new FileCleanUp(fileName, true);

        FileSys.File.WriteAllText(fileName, "test");

        FileSys.FileInfo.New(fileName).IsReadOnly = true;

        // Act
        var act = () => fcu.Dispose();

        // Assert
        act
            .Should()
            .Throw<UnauthorizedAccessException>();
    }

    [Fact]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public void Dispose_FileExistsNoThrow_NotThrowsExceptionAndFileIsDeleted()
    {
        var fileName = FileSys.Path.Combine(FileSys.Path.GetTempPath(), FileSys.Path.GetRandomFileName());

        FileSys.Directory.CreateDirectory(FileSys.Path.GetDirectoryName(fileName)!);

        // Arrange
        var fcu = new FileCleanUp(fileName);

        FileSys.File.WriteAllText(fileName, "test");

        // Act
        fcu.Dispose();

        // Assert
        FileSys.File.Exists(fileName)
            .Should()
            .BeFalse();
    }
}

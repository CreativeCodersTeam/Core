using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using CreativeCoders.Core.IO;
using CreativeCoders.UnitTests;
using FluentAssertions;
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
            @"C:\");

        mockFileSystem.Install();
        
        var fileName = FileSys.Path.GetTempFileName();

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
    public void Dispose_FileNotExistsThrow_ThrowsException()
    {
        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>(),
            @"C:\");

        mockFileSystem.Install();
        
        var fileName = FileSys.Path.GetTempFileName();

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
    public void Dispose_FileExistsNoThrow_NotThrowsExceptionAndFileIsDeleted()
    {
        var fileName = FileSys.Path.GetTempFileName();

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

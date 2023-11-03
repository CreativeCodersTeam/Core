using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using CreativeCoders.Core.IO;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.IO;

public class FileCleanUpTests
{
    [Fact]
    public void Dispose_FileNotExistsNoThrow_NotThrowsException()
    {
        Directory.CreateDirectory(Path.GetTempPath());
        
        var fileName = Path.GetTempFileName();

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
        Directory.CreateDirectory(Path.GetTempPath());
        
        var fileName = Path.GetTempFileName();

        // Arrange
        using var fcu = new FileCleanUp(fileName, true);

        using var fileStream = File.OpenWrite(fileName);

        // Act
        var act = () => fcu.Dispose();

        // Assert
        act
            .Should()
            .Throw<IOException>();

        fileStream.Dispose();
    }

    [Fact]
    public void Dispose_FileExistsNoThrow_NotThrowsExceptionAndFileIsDeleted()
    {
        Directory.CreateDirectory(Path.GetTempPath());
        
        var fileName = Path.GetTempFileName();

        // Arrange
        var fcu = new FileCleanUp(fileName);

        File.WriteAllText(fileName, "test");

        // Act
        fcu.Dispose();

        // Assert
        File.Exists(fileName)
            .Should()
            .BeFalse();
    }
}

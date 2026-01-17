using System.IO.Compression;
using AwesomeAssertions;
using CreativeCoders.IO.Archives;
using CreativeCoders.IO.Archives.Tar;
using CreativeCoders.IO.Archives.Zip;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.IO.UnitTests.Archives;

public class ArchiveWriterFactoryTests
{
    [Fact]
    public void CreateTarWriter_WithStream_ReturnsWriterFromTarFactory()
    {
        // Arrange
        var zipFactory = A.Fake<IZipArchiveWriterFactory>();
        var tarFactory = A.Fake<ITarArchiveWriterFactory>();
        var factory = new ArchiveWriterFactory(zipFactory, tarFactory);
        using var stream = new MemoryStream();
        var expectedWriter = A.Fake<ITarArchiveWriter>();

        A.CallTo(() => tarFactory.CreateWriter(stream, true, CompressionLevel.Optimal, false))
            .Returns(expectedWriter);

        // Act
        var result = factory.CreateTarWriter(stream);

        // Assert
        result
            .Should().BeSameAs(expectedWriter);
    }

    [Fact]
    public void CreateTarWriter_WithFileName_ReturnsWriterFromTarFactory()
    {
        // Arrange
        var zipFactory = A.Fake<IZipArchiveWriterFactory>();
        var tarFactory = A.Fake<ITarArchiveWriterFactory>();
        var factory = new ArchiveWriterFactory(zipFactory, tarFactory);
        const string fileName = "test.tar";
        var expectedWriter = A.Fake<ITarArchiveWriter>();

        A.CallTo(() => tarFactory.CreateWriter(fileName, null, CompressionLevel.Optimal))
            .Returns(expectedWriter);

        // Act
        var result = factory.CreateTarWriter(fileName);

        // Assert
        result
            .Should().BeSameAs(expectedWriter);
    }

    [Fact]
    public void CreateTarGzWriter_WithStream_ReturnsWriterFromTarFactory()
    {
        // Arrange
        var zipFactory = A.Fake<IZipArchiveWriterFactory>();
        var tarFactory = A.Fake<ITarArchiveWriterFactory>();
        var factory = new ArchiveWriterFactory(zipFactory, tarFactory);
        using var stream = new MemoryStream();
        var expectedWriter = A.Fake<ITarArchiveWriter>();

        A.CallTo(() => tarFactory.CreateWriter(stream, true, CompressionLevel.Optimal, false))
            .Returns(expectedWriter);

        // Act
        var result = factory.CreateTarGzWriter(stream);

        // Assert
        result
            .Should().BeSameAs(expectedWriter);
    }

    [Fact]
    public void CreateTarGzWriter_WithFileName_ReturnsWriterFromTarFactory()
    {
        // Arrange
        var zipFactory = A.Fake<IZipArchiveWriterFactory>();
        var tarFactory = A.Fake<ITarArchiveWriterFactory>();
        var factory = new ArchiveWriterFactory(zipFactory, tarFactory);
        const string fileName = "test.tar.gz";
        var expectedWriter = A.Fake<ITarArchiveWriter>();

        A.CallTo(() => tarFactory.CreateWriter(fileName, true, CompressionLevel.Optimal))
            .Returns(expectedWriter);

        // Act
        var result = factory.CreateTarGzWriter(fileName);

        // Assert
        result
            .Should().BeSameAs(expectedWriter);
    }

    [Fact]
    public void CreateZipWriter_WithStream_ReturnsWriterFromZipFactory()
    {
        // Arrange
        var zipFactory = A.Fake<IZipArchiveWriterFactory>();
        var tarFactory = A.Fake<ITarArchiveWriterFactory>();
        var factory = new ArchiveWriterFactory(zipFactory, tarFactory);
        using var stream = new MemoryStream();
        var expectedWriter = A.Fake<IZipArchiveWriter>();

        A.CallTo(() => zipFactory.CreateWriter(stream, CompressionLevel.Optimal, false))
            .Returns(expectedWriter);

        // Act
        var result = factory.CreateZipWriter(stream);

        // Assert
        result
            .Should().BeSameAs(expectedWriter);
    }

    [Fact]
    public void CreateZipWriter_WithFileName_ReturnsWriterFromZipFactory()
    {
        // Arrange
        var zipFactory = A.Fake<IZipArchiveWriterFactory>();
        var tarFactory = A.Fake<ITarArchiveWriterFactory>();
        var factory = new ArchiveWriterFactory(zipFactory, tarFactory);
        const string fileName = "test.zip";
        var expectedWriter = A.Fake<IZipArchiveWriter>();

        A.CallTo(() => zipFactory.CreateWriter(fileName, CompressionLevel.Optimal, true))
            .Returns(expectedWriter);

        // Act
        var result = factory.CreateZipWriter(fileName);

        // Assert
        result
            .Should().BeSameAs(expectedWriter);
    }
}

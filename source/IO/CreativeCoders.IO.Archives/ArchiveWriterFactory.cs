using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using CreativeCoders.Core;
using CreativeCoders.IO.Archives.Tar;
using CreativeCoders.IO.Archives.Zip;

namespace CreativeCoders.IO.Archives;

[ExcludeFromCodeCoverage]
public class ArchiveWriterFactory(
    IZipArchiveWriterFactory zipArchiveWriterFactory,
    ITarArchiveWriterFactory tarArchiveWriterFactory) : IArchiveWriterFactory
{
    private readonly IZipArchiveWriterFactory _zipArchiveWriterFactory =
        Ensure.NotNull(zipArchiveWriterFactory);

    private readonly ITarArchiveWriterFactory _tarArchiveWriterFactory =
        Ensure.NotNull(tarArchiveWriterFactory);

    public ITarArchiveWriter CreateTarWriter(Stream outputStream, bool useGZipCompression = true,
        CompressionLevel compressionLevel = CompressionLevel.Optimal)
    {
        return _tarArchiveWriterFactory.CreateWriter(outputStream, useGZipCompression, compressionLevel);
    }

    public ITarArchiveWriter CreateTarWriter(string archiveFileName, bool? useGZipCompression = null,
        CompressionLevel compressionLevel = CompressionLevel.Optimal)
    {
        return _tarArchiveWriterFactory.CreateWriter(archiveFileName, useGZipCompression, compressionLevel);
    }

    public ITarArchiveWriter CreateTarGzWriter(Stream outputStream,
        CompressionLevel compressionLevel = CompressionLevel.Optimal)
    {
        return CreateTarWriter(outputStream, true, compressionLevel);
    }

    public ITarArchiveWriter CreateTarGzWriter(string archiveFileName,
        CompressionLevel compressionLevel = CompressionLevel.Optimal)
    {
        return CreateTarWriter(archiveFileName, true, compressionLevel);
    }

    public IZipArchiveWriter CreateZipWriter(Stream outputStream,
        CompressionLevel compressionLevel = CompressionLevel.Optimal)
    {
        return _zipArchiveWriterFactory.CreateWriter(outputStream, compressionLevel);
    }

    public IZipArchiveWriter CreateZipWriter(string archiveFileName,
        CompressionLevel compressionLevel = CompressionLevel.Optimal,
        bool overwriteExisting = true)
    {
        return _zipArchiveWriterFactory.CreateWriter(archiveFileName, compressionLevel, overwriteExisting);
    }
}

using System.IO.Compression;
using CreativeCoders.IO.Archives.Tar;
using CreativeCoders.IO.Archives.Zip;

namespace CreativeCoders.IO.Archives;

public interface IArchiveWriterFactory
{
    ITarArchiveWriter CreateTarWriter(Stream outputStream, bool useGZipCompression = true,
        CompressionLevel compressionLevel = CompressionLevel.Optimal);

    ITarArchiveWriter CreateTarWriter(string archiveFileName, bool? useGZipCompression = null,
        CompressionLevel compressionLevel = CompressionLevel.Optimal);

    ITarArchiveWriter CreateTarGzWriter(Stream outputStream,
        CompressionLevel compressionLevel = CompressionLevel.Optimal);

    ITarArchiveWriter CreateTarGzWriter(string archiveFileName,
        CompressionLevel compressionLevel = CompressionLevel.Optimal);

    IZipArchiveWriter CreateZipWriter(Stream outputStream,
        CompressionLevel compressionLevel = CompressionLevel.Optimal, bool leaveOpen = false);

    IZipArchiveWriter CreateZipWriter(string archiveFileName,
        CompressionLevel compressionLevel = CompressionLevel.Optimal, bool overwriteExisting = true);
}

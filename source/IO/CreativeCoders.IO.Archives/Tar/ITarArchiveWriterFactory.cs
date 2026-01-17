using System.IO.Compression;

namespace CreativeCoders.IO.Archives.Tar;

public interface ITarArchiveWriterFactory
{
    ITarArchiveWriter CreateWriter(Stream outputStream, bool useGZipCompression = true,
        CompressionLevel compressionLevel = CompressionLevel.Optimal, bool leaveOpen = false);

    ITarArchiveWriter CreateWriter(string archiveFileName, bool? useGZipCompression = null,
        CompressionLevel compressionLevel = CompressionLevel.Optimal);
}

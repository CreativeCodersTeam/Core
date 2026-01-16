using System.IO.Compression;

namespace CreativeCoders.IO.Archives.Zip;

public interface IZipArchiveWriterFactory
{
    IZipArchiveWriter CreateWriter(Stream outputStream, CompressionLevel compressionLevel = CompressionLevel.Optimal);

    IZipArchiveWriter CreateWriter(string archiveFileName, CompressionLevel compressionLevel = CompressionLevel.Optimal, bool overwriteExisting = true);
}

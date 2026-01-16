using System.IO.Compression;

namespace CreativeCoders.IO.Archives.Zip;

public class ZipArchiveWriterFactory : IZipArchiveWriterFactory
{
    public IZipArchiveWriter CreateWriter(Stream outputStream,
        CompressionLevel compressionLevel = CompressionLevel.Optimal)
    {
        return new ZipArchiveWriter(outputStream, compressionLevel);
    }

    public IZipArchiveWriter CreateWriter(string archiveFileName,
        CompressionLevel compressionLevel = CompressionLevel.Optimal, bool overwriteExisting = true)
    {
        return CreateWriter(overwriteExisting ? File.Create(archiveFileName) : File.OpenWrite(archiveFileName),
            compressionLevel);
    }
}

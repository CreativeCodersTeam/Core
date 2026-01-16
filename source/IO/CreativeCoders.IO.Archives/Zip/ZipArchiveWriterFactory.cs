using System.IO.Compression;

namespace CreativeCoders.IO.Archives.Zip;

public class ZipArchiveWriterFactory : IZipArchiveWriterFactory
{
    public IZipArchiveWriter CreateWriter(Stream outputStream,
        CompressionLevel compressionLevel = CompressionLevel.Optimal, bool leaveOpen = false)
    {
        return new ZipArchiveWriter(outputStream, compressionLevel, leaveOpen);
    }

    public IZipArchiveWriter CreateWriter(string archiveFileName,
        CompressionLevel compressionLevel = CompressionLevel.Optimal, bool overwriteExisting = true)
    {
        return CreateWriter(
            overwriteExisting ? File.Create(archiveFileName) : File.OpenWrite(archiveFileName),
            compressionLevel);
    }
}

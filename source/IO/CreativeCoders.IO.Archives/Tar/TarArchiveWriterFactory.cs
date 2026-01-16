using System.IO.Compression;

namespace CreativeCoders.IO.Archives.Tar;

public class TarArchiveWriterFactory : ITarArchiveWriterFactory
{
    public ITarArchiveWriter CreateWriter(Stream outputStream, bool useGZipCompression = true,
        CompressionLevel compressionLevel = CompressionLevel.Optimal)
    {
        return new TarArchiveWriter(useGZipCompression ? new GZipStream(outputStream, compressionLevel) : outputStream);
    }

    public ITarArchiveWriter CreateWriter(string archiveFileName, bool? useGZipCompression = null,
        CompressionLevel compressionLevel = CompressionLevel.Optimal)
    {
        return CreateWriter(
            File.Create(archiveFileName), useGZipCompression ??
                                          TarFileHelper.IsGZipFile(archiveFileName), compressionLevel);
    }
}

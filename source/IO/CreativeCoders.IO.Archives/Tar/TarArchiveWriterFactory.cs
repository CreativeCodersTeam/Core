using System.IO.Abstractions;
using System.IO.Compression;
using CreativeCoders.Core;

namespace CreativeCoders.IO.Archives.Tar;

public class TarArchiveWriterFactory(IFileSystem fileSystem) : ITarArchiveWriterFactory
{
    private readonly IFileSystem _fileSystem = Ensure.NotNull(fileSystem);

    public ITarArchiveWriter CreateWriter(Stream outputStream, bool useGZipCompression = true,
        CompressionLevel compressionLevel = CompressionLevel.Optimal)
    {
        return new TarArchiveWriter(useGZipCompression
            ? new GZipStream(outputStream, compressionLevel)
            : outputStream);
    }

    public ITarArchiveWriter CreateWriter(string archiveFileName, bool? useGZipCompression = null,
        CompressionLevel compressionLevel = CompressionLevel.Optimal)
    {
        return CreateWriter(
            _fileSystem.File.Create(archiveFileName), useGZipCompression ??
                                                      TarFileHelper.IsGZipFile(archiveFileName),
            compressionLevel);
    }
}

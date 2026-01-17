using System.IO.Abstractions;
using System.IO.Compression;
using CreativeCoders.Core;

namespace CreativeCoders.IO.Archives.Tar;

public class TarArchiveWriterFactory(IFileSystem fileSystem) : ITarArchiveWriterFactory
{
    private readonly IFileSystem _fileSystem = Ensure.NotNull(fileSystem);

    public ITarArchiveWriter CreateWriter(Stream outputStream, bool useGZipCompression = true,
        CompressionLevel compressionLevel = CompressionLevel.Optimal, bool leaveOpen = false)
    {
        return new TarArchiveWriter(useGZipCompression
            ? new GZipStream(outputStream, compressionLevel, leaveOpen)
            : outputStream, leaveOpen);
    }

    public ITarArchiveWriter CreateWriter(string archiveFileName, bool? useGZipCompression = null,
        CompressionLevel compressionLevel = CompressionLevel.Optimal)
    {
        var dirName = _fileSystem.Path.GetDirectoryName(archiveFileName);

        if (!string.IsNullOrWhiteSpace(dirName))
        {
            _fileSystem.Directory.CreateDirectory(dirName);
        }

        return CreateWriter(
            _fileSystem.File.Create(archiveFileName), useGZipCompression ??
                                                      TarFileHelper.IsGZipFile(archiveFileName),
            compressionLevel);
    }
}

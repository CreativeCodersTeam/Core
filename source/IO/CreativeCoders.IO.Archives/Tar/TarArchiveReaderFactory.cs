using System.IO.Abstractions;
using CreativeCoders.Core;

namespace CreativeCoders.IO.Archives.Tar;

public class TarArchiveReaderFactory(IFileSystem fileSystem) : ITarArchiveReaderFactory
{
    private readonly IFileSystem _fileSystem = Ensure.NotNull(fileSystem);

    public ITarArchiveReader CreateReader(Stream inputStream, bool useGZipCompression = true)
    {
        return useGZipCompression
            ? new TarGzArchiveReader(inputStream, _fileSystem)
            : new TarArchiveReader(inputStream, _fileSystem);
    }

    public ITarArchiveReader CreateReader(string archiveFileName, bool? useGZipCompression = null)
    {
        return CreateReader(
            _fileSystem.File.OpenRead(archiveFileName), useGZipCompression ??
                                                        TarFileHelper.IsGZipFile(archiveFileName));
    }
}

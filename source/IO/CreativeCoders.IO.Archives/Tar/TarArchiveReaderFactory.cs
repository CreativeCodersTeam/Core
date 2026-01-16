using CreativeCoders.Core.IO;

namespace CreativeCoders.IO.Archives.Tar;

public class TarArchiveReaderFactory : ITarArchiveReaderFactory
{
    public ITarArchiveReader CreateReader(Stream inputStream, bool useGZipCompression = true)
    {
        return useGZipCompression
            ? new TarGzArchiveReader(inputStream)
            : new TarArchiveReader(inputStream);
    }

    public ITarArchiveReader CreateReader(string archiveFileName, bool? useGZipCompression = null)
    {
        return CreateReader(
            FileSys.File.OpenRead(archiveFileName), useGZipCompression ??
                                                    TarFileHelper.IsGZipFile(archiveFileName));
    }
}

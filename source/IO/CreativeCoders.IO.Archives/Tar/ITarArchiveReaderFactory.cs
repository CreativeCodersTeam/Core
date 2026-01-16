namespace CreativeCoders.IO.Archives.Tar;

public interface ITarArchiveReaderFactory
{
    ITarArchiveReader CreateReader(Stream inputStream, bool useGZipCompression = true);

    ITarArchiveReader CreateReader(string archiveFileName, bool? useGZipCompression = null);
}

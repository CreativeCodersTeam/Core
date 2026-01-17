namespace CreativeCoders.IO.Archives.Tar;

public interface ITarArchiveWriter : IArchiveWriter
{
    Task AddFileAsync(Stream stream, string fileNameInArchive, Action<TarEntryInfo> configureEntry);

    Task AddFileAsync(Stream stream, string fileNameInArchive, TarEntryInfo entryInfo);
}

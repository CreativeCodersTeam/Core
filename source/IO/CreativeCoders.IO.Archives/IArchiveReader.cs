namespace CreativeCoders.IO.Archives;

public interface IArchiveReader : IDisposable, IAsyncDisposable
{
    IAsyncEnumerable<ArchiveEntry> GetEntriesAsync();

    IEnumerable<ArchiveEntry> GetEntries();

    Task<Stream> OpenEntryStreamAsync(ArchiveEntry entry);

    Task ExtractFileAsync(ArchiveEntry entry, string outputFilePath, bool overwriteExisting = true);

    Task<string> ExtractFileWithPathAsync(ArchiveEntry entry, string outputBaseDirectory,
        bool overwriteExisting = true);

    Task ExtractAllAsync(string outputBaseDirectory, bool overwriteExisting = true);
}

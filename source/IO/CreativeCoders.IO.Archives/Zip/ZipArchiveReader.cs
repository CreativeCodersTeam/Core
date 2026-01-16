using System.IO.Compression;

namespace CreativeCoders.IO.Archives.Zip;

public sealed class ZipArchiveReader(ZipArchive zipArchive) : IZipArchiveReader
{
    private readonly ZipArchive _zipArchive = zipArchive ?? throw new ArgumentNullException(nameof(zipArchive));

    public static ZipArchiveReader Create(Stream inputStream)
    {
        var zipArchive = new ZipArchive(inputStream, ZipArchiveMode.Read, false);

        return new ZipArchiveReader(zipArchive);
    }

    public static async Task<ZipArchiveReader> CreateAsync(Stream inputStream)
    {
        var zipArchive = await ZipArchive.CreateAsync(inputStream, ZipArchiveMode.Read, false, null)
            .ConfigureAwait(false);

        return new ZipArchiveReader(zipArchive);
    }

    public IAsyncEnumerable<ArchiveEntry> GetEntriesAsync()
    {
        return GetEntries().ToAsyncEnumerable();
    }

    public IEnumerable<ArchiveEntry> GetEntries()
    {
        return _zipArchive.Entries
            .Select(x =>
                new ArchiveEntry(x.FullName));
    }

    public Task<Stream> OpenEntryStreamAsync(ArchiveEntry entry)
    {
        var zipEntry = _zipArchive.GetEntry(entry.FullName);

        return zipEntry == null
            ? throw new FileNotFoundException($"Entry '{entry.FullName}' not found in the zip archive.")
            : zipEntry.OpenAsync();
    }

    public Task ExtractFileAsync(ArchiveEntry entry, string outputFilePath, bool overwriteExisting = true)
    {
        var zipEntry = _zipArchive.GetEntry(entry.FullName);

        return zipEntry == null
            ? throw new FileNotFoundException($"Entry '{entry.FullName}' not found in the zip archive.")
            : ExtractFileCoreAsync(zipEntry, outputFilePath, overwriteExisting);
    }

    private static Task ExtractFileCoreAsync(ZipArchiveEntry zipEntry, string outputFilePath, bool overwriteExisting)
    {
        var outputDirectory = Path.GetDirectoryName(outputFilePath);
        if (outputDirectory != null)
        {
            Directory.CreateDirectory(outputDirectory);
        }

        return zipEntry.ExtractToFileAsync(outputFilePath, overwriteExisting);
    }

    public async Task<string> ExtractFileWithPathAsync(ArchiveEntry entry, string outputBaseDirectory,
        bool overwriteExisting = true)
    {
        var outputFileName = Path.Combine(outputBaseDirectory, entry.FullName);

        await ExtractFileAsync(entry, outputFileName, overwriteExisting).ConfigureAwait(false);

        return outputFileName;
    }

    public Task ExtractAllAsync(string outputBaseDirectory, bool overwriteExisting = true)
    {
        return _zipArchive.ExtractToDirectoryAsync(outputBaseDirectory, overwriteExisting);
    }

    public void Dispose()
    {
        _zipArchive.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _zipArchive.DisposeAsync().ConfigureAwait(false);
    }
}

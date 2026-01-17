using System.Formats.Tar;
using System.IO.Compression;
using CreativeCoders.Core;

namespace CreativeCoders.IO.Archives.Tar;

public sealed class TarGzArchiveReader : ITarArchiveReader
{
    private TarReader _tarReader;

    private bool _needsReset;

    private Stream _gzInputStream;

    private readonly Stream _sourceStream;

    public TarGzArchiveReader(Stream inputStream)
    {
        _sourceStream = Ensure.NotNull(inputStream);
        _gzInputStream = new GZipStream(_sourceStream, CompressionMode.Decompress, true);
        _tarReader = new TarReader(_gzInputStream, true);
    }

    private void ResetTarReader()
    {
        if (!_needsReset)
        {
            _needsReset = true;
            return;
        }

        _tarReader.Dispose();

        _gzInputStream.Dispose();
        _sourceStream.Seek(0, SeekOrigin.Begin);
        _gzInputStream = new GZipStream(_sourceStream, CompressionMode.Decompress, true);

        _tarReader = new TarReader(_gzInputStream, true);
    }

    private async Task ResetTarReaderAsync()
    {
        if (!_needsReset)
        {
            _needsReset = true;
            return;
        }

        await _tarReader.DisposeAsync().ConfigureAwait(false);

        await _gzInputStream.DisposeAsync().ConfigureAwait(false);
        _sourceStream.Seek(0, SeekOrigin.Begin);
        _gzInputStream = new GZipStream(_sourceStream, CompressionMode.Decompress, true);

        _tarReader = new TarReader(_gzInputStream, true);
    }

    private async IAsyncEnumerable<TarEntry> GetAllTarEntriesAsync()
    {
        await ResetTarReaderAsync().ConfigureAwait(false);

        var tarEntry = await _tarReader.GetNextEntryAsync().ConfigureAwait(false);

        while (tarEntry is not null)
        {
            yield return tarEntry;

            tarEntry = await _tarReader.GetNextEntryAsync().ConfigureAwait(false);
        }
    }

    public async IAsyncEnumerable<ArchiveEntry> GetEntriesAsync()
    {
        await foreach (var tarEntry in GetAllTarEntriesAsync().ConfigureAwait(false))
        {
            yield return new ArchiveEntry(tarEntry.Name);
        }
    }

    public IEnumerable<ArchiveEntry> GetEntries()
    {
        ResetTarReader();

        var tarEntry = _tarReader.GetNextEntry();

        while (tarEntry is not null)
        {
            yield return new ArchiveEntry(tarEntry.Name);

            tarEntry = _tarReader.GetNextEntry();
        }
    }

    private async Task<TarEntry?> GetTarEntryAsync(ArchiveEntry entry)
    {
        await ResetTarReaderAsync().ConfigureAwait(false);

        var tarEntry = await _tarReader.GetNextEntryAsync().ConfigureAwait(false);

        while (tarEntry is not null)
        {
            if (tarEntry.Name == entry.FullName)
            {
                return tarEntry;
            }

            tarEntry = await _tarReader.GetNextEntryAsync().ConfigureAwait(false);
        }

        return null;
    }

    public async Task<Stream> OpenEntryStreamAsync(ArchiveEntry entry, bool copyData = false)
    {
        var tarEntry = await GetTarEntryAsync(entry).ConfigureAwait(false);

        if (tarEntry == null)
        {
            throw new FileNotFoundException($"Entry '{entry.FullName}' not found in the archive.");
        }

        var entryDataStream = tarEntry.DataStream ??
                              throw new FileNotFoundException($"Entry '{entry.FullName}' has no stream.");

        if (!copyData)
        {
            return entryDataStream;
        }

        var memoryStream = new MemoryStream();
        await entryDataStream.CopyToAsync(memoryStream).ConfigureAwait(false);
        memoryStream.Position = 0;

        return memoryStream;
    }

    public async Task ExtractFileAsync(ArchiveEntry entry, string outputFilePath,
        bool overwriteExisting = true)
    {
        var tarEntry = await GetTarEntryAsync(entry).ConfigureAwait(false);

        if (tarEntry == null)
        {
            throw new FileNotFoundException($"Entry '{entry.FullName}' not found in the archive.");
        }

        await ExtractFileCoreAsync(tarEntry, outputFilePath, overwriteExisting).ConfigureAwait(false);
    }

    private static Task ExtractFileCoreAsync(TarEntry tarEntry, string outputFilePath, bool overwriteExisting)
    {
        var outputDirectory = Path.GetDirectoryName(outputFilePath);
        if (outputDirectory != null)
        {
            Directory.CreateDirectory(outputDirectory);
        }

        return tarEntry.ExtractToFileAsync(outputFilePath, overwriteExisting);
    }

    public async Task<string> ExtractFileWithPathAsync(ArchiveEntry entry, string outputBaseDirectory,
        bool overwriteExisting = true)
    {
        var outputFileName = Path.Combine(outputBaseDirectory, entry.FullName);

        await ExtractFileAsync(entry, outputFileName, overwriteExisting).ConfigureAwait(false);

        return outputFileName;
    }

    public async Task ExtractAllAsync(string outputBaseDirectory, bool overwriteExisting = true)
    {
        await ResetTarReaderAsync().ConfigureAwait(false);

        var tarEntry = await _tarReader.GetNextEntryAsync().ConfigureAwait(false);

        while (tarEntry is not null)
        {
            var outputFileName = Path.Combine(outputBaseDirectory, tarEntry.Name);

            await ExtractFileCoreAsync(tarEntry, outputFileName, overwriteExisting).ConfigureAwait(false);

            tarEntry = await _tarReader.GetNextEntryAsync().ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
        _tarReader.Dispose();
        _gzInputStream.Dispose();
        _sourceStream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _tarReader.DisposeAsync().ConfigureAwait(false);
        await _gzInputStream.DisposeAsync().ConfigureAwait(false);
        await _sourceStream.DisposeAsync().ConfigureAwait(false);
    }
}

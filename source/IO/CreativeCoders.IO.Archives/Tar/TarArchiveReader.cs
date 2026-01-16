using System.Formats.Tar;

namespace CreativeCoders.IO.Archives.Tar;

public sealed class TarArchiveReader(Stream inputStream) : ITarArchiveReader
{
    private TarReader _tarReader = new TarReader(inputStream, true);

    private bool _needsReset;

    private void ResetTarReader()
    {
        if (!_needsReset)
        {
            _needsReset = true;
            return;
        }

        _tarReader.Dispose();

        if (inputStream.CanSeek)
        {
            inputStream.Seek(0, SeekOrigin.Begin);
        }
        else
        {
            throw new InvalidOperationException("The input stream is not seekable.");
        }

        _tarReader = new TarReader(inputStream, true);
    }

    private async Task ResetTarReaderAsync()
    {
        if (!_needsReset)
        {
            _needsReset = true;
            return;
        }

        await _tarReader.DisposeAsync().ConfigureAwait(false);

        if (inputStream.CanSeek)
        {
            inputStream.Seek(0, SeekOrigin.Begin);
        }
        else
        {
            throw new InvalidOperationException("The input stream is not seekable.");
        }

        _tarReader = new TarReader(inputStream, true);
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

    public Task<Stream> OpenEntryStreamAsync(ArchiveEntry entry)
    {
        throw new NotImplementedException();
    }

    public async Task ExtractFileAsync(ArchiveEntry entry, string outputFilePath, bool overwriteExisting = true)
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
        inputStream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _tarReader.DisposeAsync().ConfigureAwait(false);
        await inputStream.DisposeAsync().ConfigureAwait(false);
    }
}

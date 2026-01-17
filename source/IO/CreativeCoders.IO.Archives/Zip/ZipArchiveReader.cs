using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.IO.Compression;
using CreativeCoders.Core;
using CreativeCoders.Core.IO;

namespace CreativeCoders.IO.Archives.Zip;

public sealed class ZipArchiveReader(ZipArchive zipArchive, IFileSystem fileSystem) : IZipArchiveReader
{
    private readonly IFileSystem _fileSystem = Ensure.NotNull(fileSystem);

    private readonly ZipArchive _zipArchive = Ensure.NotNull(zipArchive);

    public static ZipArchiveReader Create(Stream inputStream, IFileSystem? fileSystem = null)
    {
        var zipArchive = new ZipArchive(inputStream, ZipArchiveMode.Read, false);

        return new ZipArchiveReader(zipArchive, fileSystem ?? new FileSystem());
    }

    public static async Task<ZipArchiveReader> CreateAsync(Stream inputStream, IFileSystem? fileSystem = null)
    {
        var zipArchive = await ZipArchive.CreateAsync(inputStream, ZipArchiveMode.Read, false, null)
            .ConfigureAwait(false);

        return new ZipArchiveReader(zipArchive, fileSystem ?? new FileSystem());
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

    public async Task<Stream> OpenEntryStreamAsync(ArchiveEntry entry, bool copyData = false)
    {
        var zipEntry = _zipArchive.GetEntry(entry.FullName);

        if (zipEntry == null)
        {
            throw new FileNotFoundException($"Entry '{entry.FullName}' not found in the zip archive.");
        }

        if (!copyData)
        {
            return await zipEntry.OpenAsync().ConfigureAwait(false);
        }

        var memoryStream = new MemoryStream();
        var zipEntryStream = await zipEntry.OpenAsync().ConfigureAwait(false);
        await using var zipEntryStreamDisposable = zipEntryStream.ConfigureAwait(false);

        await zipEntryStream.CopyToAsync(memoryStream).ConfigureAwait(false);

        memoryStream.Position = 0;
        return memoryStream;
    }

    public Task ExtractFileAsync(ArchiveEntry entry, string outputFilePath, bool overwriteExisting = true)
    {
        var zipEntry = _zipArchive.GetEntry(entry.FullName);

        return zipEntry == null
            ? throw new FileNotFoundException($"Entry '{entry.FullName}' not found in the zip archive.")
            : ExtractFileCoreAsync(zipEntry, outputFilePath, overwriteExisting);
    }

    private Task ExtractFileCoreAsync(ZipArchiveEntry zipEntry, string outputFilePath,
        bool overwriteExisting)
    {
        var outputDirectory = _fileSystem.Path.GetDirectoryName(outputFilePath);
        if (outputDirectory != null)
        {
            _fileSystem.Directory.CreateDirectory(outputDirectory);
        }

        return zipEntry.ExtractToFileAsync(outputFilePath, overwriteExisting);
    }

    public async Task<string> ExtractFileWithPathAsync(ArchiveEntry entry, string outputBaseDirectory,
        bool overwriteExisting = true)
    {
        _fileSystem.Path.EnsureSafe(outputBaseDirectory, entry.FullName);

        var outputFileName = _fileSystem.Path.Combine(outputBaseDirectory, entry.FullName);

        await ExtractFileAsync(entry, outputFileName, overwriteExisting).ConfigureAwait(false);

        return outputFileName;
    }

    public Task ExtractAllAsync(string outputBaseDirectory, bool overwriteExisting = true)
    {
        return _zipArchive.ExtractToDirectoryAsync(outputBaseDirectory, overwriteExisting);
    }

    [ExcludeFromCodeCoverage]
    public void Dispose()
    {
        _zipArchive.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _zipArchive.DisposeAsync().ConfigureAwait(false);
    }
}

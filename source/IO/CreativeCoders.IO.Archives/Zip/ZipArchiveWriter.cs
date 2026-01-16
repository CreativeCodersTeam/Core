using System.IO.Compression;

namespace CreativeCoders.IO.Archives.Zip;

public sealed class ZipArchiveWriter(
    Stream outputStream,
    CompressionLevel defaultCompressionLevel) : IZipArchiveWriter
{
    private readonly ZipArchive _archive = new ZipArchive(outputStream, ZipArchiveMode.Create);

    public Task AddFileAsync(string fileName, string fileNameInArchive, CompressionLevel compressionLevel)
    {
        return _archive.CreateEntryFromFileAsync(fileName, fileNameInArchive, compressionLevel);
    }

    public async Task AddFileAsync(Stream stream, string fileNameInArchive, CompressionLevel compressionLevel)
    {
        await using var fileStream = await _archive.CreateEntry(fileNameInArchive, compressionLevel).OpenAsync()
            .ConfigureAwait(false);

        await stream.CopyToAsync(fileStream).ConfigureAwait(false);
    }

    public async Task AddFromDirectoryAsync(string path, string basePathToRemove, CompressionLevel compressionLevel)
    {
        var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var entryName = file[basePathToRemove.Length..].TrimStart(Path.DirectorySeparatorChar);
            await AddFileAsync(file, entryName, compressionLevel).ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
        _archive.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _archive.DisposeAsync().ConfigureAwait(false);
    }

    public Task AddFileAsync(string fileName, string fileNameInArchive)
    {
        return AddFileAsync(fileName, fileNameInArchive, defaultCompressionLevel);
    }

    public Task AddFileAsync(Stream stream, string fileNameInArchive)
    {
        return AddFileAsync(stream, fileNameInArchive, defaultCompressionLevel);
    }

    public Task AddFromDirectoryAsync(string path, string basePathToRemove)
    {
        return AddFromDirectoryAsync(path, basePathToRemove, defaultCompressionLevel);
    }
}

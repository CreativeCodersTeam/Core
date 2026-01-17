using System.IO.Abstractions;
using CreativeCoders.Core;

namespace CreativeCoders.IO.Archives.Zip;

public class ZipArchiveReaderFactory(IFileSystem fileSystem) : IZipArchiveReaderFactory
{
    private readonly IFileSystem _fileSystem = Ensure.NotNull(fileSystem);

    public IZipArchiveReader CreateReader(Stream inputStream)
    {
        return ZipArchiveReader.Create(inputStream, _fileSystem);
    }

    public IZipArchiveReader CreateReader(string archiveFileName)
    {
        return CreateReader(_fileSystem.File.OpenRead(archiveFileName));
    }

    public async Task<IZipArchiveReader> CreateReaderAsync(Stream inputStream)
    {
        return await ZipArchiveReader.CreateAsync(inputStream, _fileSystem).ConfigureAwait(false);
    }

    public Task<IZipArchiveReader> CreateReaderAsync(string archiveFileName)
    {
        return CreateReaderAsync(_fileSystem.File.OpenRead(archiveFileName));
    }
}

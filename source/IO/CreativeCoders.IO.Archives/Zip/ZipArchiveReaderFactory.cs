namespace CreativeCoders.IO.Archives.Zip;

public class ZipArchiveReaderFactory : IZipArchiveReaderFactory
{
    public IZipArchiveReader CreateReader(Stream inputStream)
    {
        return ZipArchiveReader.Create(inputStream);
    }

    public IZipArchiveReader CreateReader(string archiveFileName)
    {
        return ZipArchiveReader.Create(File.OpenRead(archiveFileName));
    }

    public async Task<IZipArchiveReader> CreateReaderAsync(Stream inputStream)
    {
        return await ZipArchiveReader.CreateAsync(inputStream).ConfigureAwait(false);
    }

    public Task<IZipArchiveReader> CreateReaderAsync(string archiveFileName)
    {
        return CreateReaderAsync(File.OpenRead(archiveFileName));
    }
}

namespace CreativeCoders.IO.Archives.Zip;

public interface IZipArchiveReaderFactory
{
    IZipArchiveReader CreateReader(Stream inputStream);

    IZipArchiveReader CreateReader(string archiveFileName);

    Task<IZipArchiveReader> CreateReaderAsync(Stream inputStream);

    Task<IZipArchiveReader> CreateReaderAsync(string archiveFileName);
}

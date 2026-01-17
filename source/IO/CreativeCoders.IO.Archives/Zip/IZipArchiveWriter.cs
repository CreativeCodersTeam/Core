using System.IO.Compression;

namespace CreativeCoders.IO.Archives.Zip;

public interface IZipArchiveWriter : IArchiveWriter
{
    Task AddFileAsync(string fileName, string fileNameInArchive, CompressionLevel compressionLevel);

    Task AddFileAsync(Stream stream, string fileNameInArchive, CompressionLevel compressionLevel);

    Task AddFromDirectoryAsync(string path, string basePathToRemove, CompressionLevel compressionLevel);
}

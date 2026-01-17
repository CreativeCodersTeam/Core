using System.IO.Abstractions;
using System.IO.Compression;
using CreativeCoders.Core;

namespace CreativeCoders.IO.Archives.Zip;

public class ZipArchiveWriterFactory(IFileSystem fileSystem) : IZipArchiveWriterFactory
{
    private readonly IFileSystem _fileSystem = Ensure.NotNull(fileSystem);

    public IZipArchiveWriter CreateWriter(Stream outputStream,
        CompressionLevel compressionLevel = CompressionLevel.Optimal, bool leaveOpen = false)
    {
        return new ZipArchiveWriter(outputStream, compressionLevel, leaveOpen);
    }

    public IZipArchiveWriter CreateWriter(string archiveFileName,
        CompressionLevel compressionLevel = CompressionLevel.Optimal, bool overwriteExisting = true)
    {
        var dirName = _fileSystem.Path.GetDirectoryName(archiveFileName);

        if (!string.IsNullOrWhiteSpace(dirName))
        {
            _fileSystem.Directory.CreateDirectory(dirName);
        }

        return CreateWriter(
            overwriteExisting ? File.Create(archiveFileName) : File.OpenWrite(archiveFileName),
            compressionLevel);
    }
}

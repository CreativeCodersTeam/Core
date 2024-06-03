using System.IO.Compression;
using CreativeCoders.Core;

namespace CreativeCoders.IO.Archives;

public sealed class ZipArchiveWriter(Stream outputStream, bool disposeStream) : IZipArchiveWriter
{
    private readonly ZipArchive _zipFile = new(Ensure.NotNull(outputStream), ZipArchiveMode.Create,
        !disposeStream);

    public IZipArchiveWriter AddFromFile(string fileName, string entryName)
    {
        _zipFile.CreateEntryFromFile(fileName, entryName);

        return this;
    }

    public IZipArchiveWriter AddFromDirectory(string path, string entryPathName, bool recursive = true)
    {
        _zipFile.CreateEntriesFromDirectory(path, entryPathName, recursive);

        return this;
    }

    public void Dispose()
    {
        _zipFile.Dispose();
    }
}

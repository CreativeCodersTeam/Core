using CreativeCoders.Core.IO;

namespace CreativeCoders.IO.Archives;

public class ZipArchiveFactory : IZipArchiveFactory
{
    public IZipArchiveWriter CreateArchiveWriter(Stream outputStream, bool disposeStream = true)
    {
        return new ZipArchiveWriter(outputStream, disposeStream);
    }

    public IZipArchiveWriter CreateArchiveWriter(string fileName)
    {
        return CreateArchiveWriter(FileSys.File.Create(fileName));
    }
}

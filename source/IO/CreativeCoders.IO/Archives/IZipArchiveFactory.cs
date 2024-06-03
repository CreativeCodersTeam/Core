namespace CreativeCoders.IO.Archives;

public interface IZipArchiveFactory
{
    IZipArchiveWriter CreateArchiveWriter(Stream outputStream, bool disposeStream = true);

    IZipArchiveWriter CreateArchiveWriter(string fileName);
}

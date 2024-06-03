namespace CreativeCoders.IO.Archives;

public interface IZipArchiveWriter : IDisposable
{
    IZipArchiveWriter AddFromFile(string fileName, string entryName);

    IZipArchiveWriter AddFromDirectory(string path, string entryPathName, bool recursive = true);
}

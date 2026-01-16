namespace CreativeCoders.IO.Archives;

public interface IArchiveWriter : IDisposable, IAsyncDisposable
{
    Task AddFileAsync(string fileName, string fileNameInArchive);

    Task AddFileAsync(Stream stream, string fileNameInArchive);

    Task AddFromDirectoryAsync(string path, string basePathToRemove);
}

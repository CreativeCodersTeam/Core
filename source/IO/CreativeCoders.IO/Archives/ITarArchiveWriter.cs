using JetBrains.Annotations;

namespace CreativeCoders.IO.Archives;

[PublicAPI]
public interface ITarArchiveWriter : IAsyncDisposable
{
    Task AddFileAsync(string fileName, string fileNameInArchive);

    Task AddFileAsync(string fileNameInArchive, Stream fileContent, long contentSize, int fileMode,
        TarFileOwnerInfo fileOwnerInfo, DateTime modificationTime);

    Task AddFromDirectoryAsync(string path, string removePrefix);

    Task AddFromDirectoryAsync(string path);
}

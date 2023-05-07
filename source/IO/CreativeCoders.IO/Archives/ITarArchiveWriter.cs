using System.Text;
using JetBrains.Annotations;

namespace CreativeCoders.IO.Archives;

[PublicAPI]
public interface ITarArchiveWriter : IAsyncDisposable
{
    Task AddFileAsync(string fileName, string fileNameInArchive);

    Task AddFromDirectoryAsync(string path, string removePrefix);

    Task AddFromDirectoryAsync(string path);
}

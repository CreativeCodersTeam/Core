using System.IO.Abstractions;
using JetBrains.Annotations;

namespace CreativeCoders.Core.IO
{
    [PublicAPI]
    public interface IFileSystemEx : IFileSystem
    {
        void Install();

        FileSystemWatcherBase CreateFileSystemWatcher();

        FileSystemWatcherBase CreateFileSystemWatcher(string path);

        FileSystemWatcherBase CreateFileSystemWatcher(string path, string filter);
    }
}
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using JetBrains.Annotations;

namespace CreativeCoders.Core.IO
{
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class FileSystemEx : FileSystem, IFileSystemEx
    {
        public void Install()
        {
            FileSys.InstallFileSystemSupport(this);
        }

        public FileSystemWatcherBase CreateFileSystemWatcher()
        {
            return new FileSystemWatcherWrapper();
        }

        public FileSystemWatcherBase CreateFileSystemWatcher(string path)
        {
            return new FileSystemWatcherWrapper(path);
        }

        public FileSystemWatcherBase CreateFileSystemWatcher(string path, string filter)
        {
            return new FileSystemWatcherWrapper(path, filter);
        }
    }
}
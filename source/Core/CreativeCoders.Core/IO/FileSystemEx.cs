using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.IO;

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
        return new FileSystemWatcherWrapper(this);
    }

    public FileSystemWatcherBase CreateFileSystemWatcher(string path)
    {
        return new FileSystemWatcherWrapper(this, path);
    }

    public FileSystemWatcherBase CreateFileSystemWatcher(string path, string filter)
    {
        return new FileSystemWatcherWrapper(this, path, filter);
    }
}

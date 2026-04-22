using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.IO;

/// <summary>
/// Provides an extended file system implementation that adds file system watcher support
/// and self-installation to <see cref="FileSys"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public class FileSystemEx : FileSystem, IFileSystemEx
{
    /// <inheritdoc/>
    public void Install()
    {
        FileSys.InstallFileSystemSupport(this);
    }

    /// <inheritdoc/>
    public FileSystemWatcherBase CreateFileSystemWatcher()
    {
        return new FileSystemWatcherWrapper(this);
    }

    /// <inheritdoc/>
    public FileSystemWatcherBase CreateFileSystemWatcher(string path)
    {
        return new FileSystemWatcherWrapper(this, path);
    }

    /// <inheritdoc/>
    public FileSystemWatcherBase CreateFileSystemWatcher(string path, string filter)
    {
        return new FileSystemWatcherWrapper(this, path, filter);
    }
}

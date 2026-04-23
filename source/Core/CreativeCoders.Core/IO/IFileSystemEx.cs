using System.IO.Abstractions;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.IO;

/// <summary>
/// Extends <see cref="IFileSystem"/> with additional file system operations including
/// self-installation and file system watcher creation.
/// </summary>
[PublicAPI]
public interface IFileSystemEx : IFileSystem
{
    /// <summary>
    /// Installs this file system instance as the global <see cref="FileSys"/> provider.
    /// </summary>
    void Install();

    /// <summary>
    /// Creates a new <see cref="FileSystemWatcherBase"/> for monitoring file system changes.
    /// </summary>
    /// <returns>A new file system watcher instance.</returns>
    FileSystemWatcherBase CreateFileSystemWatcher();

    /// <summary>
    /// Creates a new <see cref="FileSystemWatcherBase"/> that monitors the specified directory.
    /// </summary>
    /// <param name="path">The directory path to monitor.</param>
    /// <returns>A new file system watcher instance for the specified path.</returns>
    FileSystemWatcherBase CreateFileSystemWatcher(string path);

    /// <summary>
    /// Creates a new <see cref="FileSystemWatcherBase"/> that monitors the specified directory with a filter.
    /// </summary>
    /// <param name="path">The directory path to monitor.</param>
    /// <param name="filter">The file name filter (e.g., <c>"*.txt"</c>).</param>
    /// <returns>A new file system watcher instance for the specified path and filter.</returns>
    FileSystemWatcherBase CreateFileSystemWatcher(string path, string filter);
}

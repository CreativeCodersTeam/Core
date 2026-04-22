using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.IO;

/// <summary>
/// Provides static access to file system operations through an <see cref="IFileSystemEx"/> instance.
/// </summary>
[ExcludeFromCodeCoverage]
[PublicAPI]
public static class FileSys
{
    /// <summary>
    /// Installs a custom <see cref="IFileSystemEx"/> implementation as the static file system provider.
    /// </summary>
    /// <param name="fileSystem">The file system implementation to use.</param>
    public static void InstallFileSystemSupport(IFileSystemEx fileSystem)
    {
        Ensure.IsNotNull(fileSystem);

        Instance = fileSystem;
    }

    /// <summary>
    /// Creates a new <see cref="FileSystemWatcherBase"/> for monitoring file system changes.
    /// </summary>
    /// <returns>A new file system watcher instance.</returns>
    public static FileSystemWatcherBase CreateFileSystemWatcher() => Instance.CreateFileSystemWatcher();

    /// <summary>
    /// Creates a new <see cref="FileSystemWatcherBase"/> that monitors the specified directory.
    /// </summary>
    /// <param name="path">The directory path to monitor.</param>
    /// <returns>A new file system watcher instance for the specified path.</returns>
    public static FileSystemWatcherBase CreateFileSystemWatcher(string path) =>
        Instance.CreateFileSystemWatcher(path);

    /// <summary>
    /// Creates a new <see cref="FileSystemWatcherBase"/> that monitors the specified directory with a filter.
    /// </summary>
    /// <param name="path">The directory path to monitor.</param>
    /// <param name="filter">The file name filter (e.g., <c>"*.txt"</c>).</param>
    /// <returns>A new file system watcher instance for the specified path and filter.</returns>
    public static FileSystemWatcherBase CreateFileSystemWatcher(string path, string filter) =>
        Instance.CreateFileSystemWatcher(path, filter);

    private static IFileSystemEx Instance { get; set; } = new FileSystemEx();

    /// <summary>
    /// Gets the <see cref="IFile"/> service for file operations.
    /// </summary>
    public static IFile File => Instance.File;

    /// <summary>
    /// Gets the <see cref="IDirectory"/> service for directory operations.
    /// </summary>
    public static IDirectory Directory => Instance.Directory;

    /// <summary>
    /// Gets the <see cref="IFileInfoFactory"/> for creating file info instances.
    /// </summary>
    public static IFileInfoFactory FileInfo => Instance.FileInfo;

    /// <summary>
    /// Gets the <see cref="IPath"/> service for path operations.
    /// </summary>
    public static IPath Path => Instance.Path;

    /// <summary>
    /// Gets the <see cref="IDirectoryInfoFactory"/> for creating directory info instances.
    /// </summary>
    public static IDirectoryInfoFactory DirectoryInfo => Instance.DirectoryInfo;

    /// <summary>
    /// Gets the <see cref="IDriveInfoFactory"/> for creating drive info instances.
    /// </summary>
    public static IDriveInfoFactory DriveInfo => Instance.DriveInfo;
}

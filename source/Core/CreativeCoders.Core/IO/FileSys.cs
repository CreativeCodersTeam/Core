using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.IO;

[ExcludeFromCodeCoverage]
[PublicAPI]
public static class FileSys
{
    private static IFileSystemEx Instance { get; set; } = new FileSystemEx();

    public static void InstallFileSystemSupport(IFileSystemEx fileSystem)
    {
        Ensure.IsNotNull(fileSystem, nameof(fileSystem));

        Instance = fileSystem;
    }

    public static IFile File => Instance.File;

    public static IDirectory Directory => Instance.Directory;

    public static IFileInfoFactory FileInfo => Instance.FileInfo;

    public static IPath Path => Instance.Path;

    public static IDirectoryInfoFactory DirectoryInfo => Instance.DirectoryInfo;

    public static IDriveInfoFactory DriveInfo => Instance.DriveInfo;

    public static FileSystemWatcherBase CreateFileSystemWatcher() => Instance.CreateFileSystemWatcher();

    public static FileSystemWatcherBase CreateFileSystemWatcher(string path) =>
        Instance.CreateFileSystemWatcher(path);

    public static FileSystemWatcherBase CreateFileSystemWatcher(string path, string filter) =>
        Instance.CreateFileSystemWatcher(path, filter);
}

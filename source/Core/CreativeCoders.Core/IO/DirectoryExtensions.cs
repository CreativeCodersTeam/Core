using System.IO.Abstractions;

#nullable enable

namespace CreativeCoders.Core.IO;

/// <summary>
/// Provides extension methods for <see cref="IDirectory"/> to ensure directories exist.
/// </summary>
public static class DirectoryExtensions
{
    /// <summary>
    /// Ensures that the specified directory exists, creating it if necessary.
    /// Does nothing if the path is <see langword="null"/> or white-space.
    /// </summary>
    /// <param name="directory">The directory service.</param>
    /// <param name="directoryPath">The directory path to ensure exists.</param>
    public static void EnsureDirectoryExists(this IDirectory directory, string? directoryPath)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            return;
        }

        directory.CreateDirectory(directoryPath);
    }

    /// <summary>
    /// Ensures that the parent directory for the specified file path exists, creating it if necessary.
    /// Does nothing if the file path is <see langword="null"/> or white-space, or if no parent directory can be determined.
    /// </summary>
    /// <param name="directory">The directory service.</param>
    /// <param name="filePath">The file path whose parent directory to ensure exists.</param>
    public static void EnsureDirectoryForFileNameExists(this IDirectory directory, string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return;
        }

        var directoryPath = directory.FileSystem.Path.GetDirectoryName(filePath);
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            return;
        }

        directory.CreateDirectory(directoryPath);
    }
}

using System.IO.Abstractions;

#nullable enable

namespace CreativeCoders.Core.IO;

public static class DirectoryExtensions
{
    public static void EnsureDirectoryExists(this IDirectory directory, string? directoryPath)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            return;
        }

        directory.CreateDirectory(directoryPath);
    }

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

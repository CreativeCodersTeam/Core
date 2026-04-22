using System;
using System.IO;
using System.IO.Abstractions;
using CreativeCoders.Core.IO;

#nullable enable

namespace CreativeCoders.Core.IO;

/// <summary>
/// Provides methods for path safety checks to prevent path traversal attacks.
/// </summary>
public static class PathSafety
{
    /// <summary>
    /// Determines whether the specified path is safe to use within the given base path.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <param name="basePath">The base path that the <paramref name="path"/> must be contained in.</param>
    /// <returns><see langword="true"/> if the path is safe; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code>
    /// var isSafe = PathSafety.IsSafe("data/config.json", "/var/www");
    /// </code>
    /// </example>
    public static bool IsSafe(string path, string basePath)
    {
        return IsSafe(FileSys.Path, path, basePath);
    }

    /// <summary>
    /// Determines whether the specified path is safe to use within the given base path.
    /// </summary>
    /// <param name="pathHelper">The <see cref="IPath"/> service for path operations.</param>
    /// <param name="path">The path to check.</param>
    /// <param name="basePath">The base path that the <paramref name="path"/> must be contained in.</param>
    /// <returns><see langword="true"/> if the path is safe; otherwise, <see langword="false"/>.</returns>
    public static bool IsSafe(IPath pathHelper, string path, string basePath)
    {
        Ensure.NotNull(pathHelper);
        Ensure.IsNotNullOrWhitespace(path);
        Ensure.IsNotNullOrWhitespace(basePath);

        var fullBasePath = pathHelper.GetFullPath(basePath);
        if (!fullBasePath.EndsWith(pathHelper.DirectorySeparatorChar) &&
            !fullBasePath.EndsWith(pathHelper.AltDirectorySeparatorChar))
        {
            fullBasePath += pathHelper.DirectorySeparatorChar;
        }

        var combinedPath = pathHelper.Combine(fullBasePath, path);
        var fullPath = pathHelper.GetFullPath(combinedPath);

        return fullPath.StartsWith(fullBasePath, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Ensures that the specified path is safe to use within the given base path.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <param name="basePath">The base path that the <paramref name="path"/> must be contained in.</param>
    /// <exception cref="UnauthorizedAccessException">The path attempts to traverse outside the base path.</exception>
    public static void EnsureSafe(string path, string basePath)
    {
        EnsureSafe(FileSys.Path, path, basePath);
    }

    /// <summary>
    /// Ensures that the specified path is safe to use within the given base path.
    /// </summary>
    /// <param name="pathHelper">The <see cref="IPath"/> service for path operations.</param>
    /// <param name="path">The path to check.</param>
    /// <param name="basePath">The base path that the <paramref name="path"/> must be contained in.</param>
    /// <exception cref="UnauthorizedAccessException">The path attempts to traverse outside the base path.</exception>
    public static void EnsureSafe(IPath pathHelper, string path, string basePath)
    {
        if (!IsSafe(pathHelper, path, basePath))
        {
            throw new UnauthorizedAccessException(
                $"The path '{path}' is not safe and attempts to leave the base path '{basePath}'.");
        }
    }
}

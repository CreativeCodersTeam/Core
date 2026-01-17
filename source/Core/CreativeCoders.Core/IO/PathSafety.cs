using System;
using System.IO;
using System.IO.Abstractions;
using CreativeCoders.Core.IO;

namespace CreativeCoders.Core.IO;

/// <summary>
/// Provides methods for path safety checks, especially to prevent path traversal attacks.
/// </summary>
public static class PathSafety
{
    /// <summary>
    /// Checks if the specified <paramref name="path"/> is safe to use within the given <paramref name="basePath"/>.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <param name="basePath">The base path that the <paramref name="path"/> must be contained in.</param>
    /// <returns><c>true</c> if the path is safe; otherwise, <c>false</c>.</returns>
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
    /// Checks if the specified <paramref name="path"/> is safe to use within the given <paramref name="basePath"/>.
    /// </summary>
    /// <param name="pathHelper">The <see cref="IPath"/> helper to use for path operations.</param>
    /// <param name="path">The path to check.</param>
    /// <param name="basePath">The base path that the <paramref name="path"/> must be contained in.</param>
    /// <returns><c>true</c> if the path is safe; otherwise, <c>false</c>.</returns>
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
    /// Ensures that the specified <paramref name="path"/> is safe to use within the given <paramref name="basePath"/>.
    /// Throws an <see cref="UnauthorizedAccessException"/> if the path is not safe.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <param name="basePath">The base path that the <paramref name="path"/> must be contained in.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the path is not safe.</exception>
    public static void EnsureSafe(string path, string basePath)
    {
        EnsureSafe(FileSys.Path, path, basePath);
    }

    /// <summary>
    /// Ensures that the specified <paramref name="path"/> is safe to use within the given <paramref name="basePath"/>.
    /// Throws an <see cref="UnauthorizedAccessException"/> if the path is not safe.
    /// </summary>
    /// <param name="pathHelper">The <see cref="IPath"/> helper to use for path operations.</param>
    /// <param name="path">The path to check.</param>
    /// <param name="basePath">The base path that the <paramref name="path"/> must be contained in.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the path is not safe.</exception>
    public static void EnsureSafe(IPath pathHelper, string path, string basePath)
    {
        if (!IsSafe(pathHelper, path, basePath))
        {
            throw new UnauthorizedAccessException(
                $"The path '{path}' is not safe and attempts to leave the base path '{basePath}'.");
        }
    }
}

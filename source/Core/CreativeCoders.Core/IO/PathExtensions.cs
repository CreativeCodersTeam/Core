using System.IO.Abstractions;
using System.Linq;

namespace CreativeCoders.Core.IO;

/// <summary>
/// Provides extension methods for <see cref="IPath"/> operations.
/// </summary>
public static class PathExtensions
{
    /// <summary>
    /// Replaces all invalid file name characters in the specified file name with the given replacement string.
    /// </summary>
    /// <param name="path">The path service.</param>
    /// <param name="fileName">The file name to sanitize.</param>
    /// <param name="replacement">The string to substitute for each invalid character.</param>
    /// <returns>A sanitized file name with invalid characters replaced.</returns>
    public static string ReplaceInvalidFileNameChars(this IPath path, string fileName, string replacement)
    {
        return path.GetInvalidFileNameChars()
            .Aggregate(fileName, (current, c) => current.Replace(c.ToString(), replacement));
    }

    /// <summary>
    /// Determines whether the specified path is safe to use within the given base path,
    /// preventing path traversal attacks.
    /// </summary>
    /// <param name="path">The path service.</param>
    /// <param name="pathToCheck">The path to validate.</param>
    /// <param name="basePath">The base path that the checked path must remain within.</param>
    /// <returns><see langword="true"/> if the path is safe; otherwise, <see langword="false"/>.</returns>
    public static bool IsSafe(this IPath path, string pathToCheck, string basePath)
    {
        return PathSafety.IsSafe(path, pathToCheck, basePath);
    }

    /// <summary>
    /// Ensures that the specified path is safe to use within the given base path,
    /// throwing an exception if a path traversal is detected.
    /// </summary>
    /// <param name="path">The path service.</param>
    /// <param name="pathToCheck">The path to validate.</param>
    /// <param name="basePath">The base path that the checked path must remain within.</param>
    /// <exception cref="System.UnauthorizedAccessException">The path attempts to traverse outside the base path.</exception>
    public static void EnsureSafe(this IPath path, string pathToCheck, string basePath)
    {
        PathSafety.EnsureSafe(path, pathToCheck, basePath);
    }
}

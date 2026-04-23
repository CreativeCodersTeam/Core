using System;

namespace CreativeCoders.Core.IO;

/// <summary>
/// Deletes a directory when disposed, providing automatic cleanup of temporary directories.
/// </summary>
/// <param name="directoryName">The path of the directory to delete on disposal.</param>
/// <param name="recursive">
/// <see langword="true"/> to delete the directory, its subdirectories, and all files; otherwise, <see langword="false"/>.
/// </param>
/// <param name="throwException">
/// <see langword="true"/> to rethrow exceptions that occur during deletion; <see langword="false"/> to suppress them.
/// </param>
public sealed class DirectoryCleanUp(string directoryName, bool recursive = true, bool throwException = false)
    : IDisposable
{
    /// <inheritdoc/>
    public void Dispose()
    {
        try
        {
            if (FileSys.Directory.Exists(DirectoryName))
            {
                FileSys.Directory.Delete(DirectoryName, recursive);
            }
        }
        catch (Exception)
        {
            if (throwException)
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Gets the path of the directory that will be deleted on disposal.
    /// </summary>
    public string DirectoryName { get; } = Ensure.IsNotNullOrWhitespace(directoryName);
}

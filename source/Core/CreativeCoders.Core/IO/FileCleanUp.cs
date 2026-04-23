using System;

namespace CreativeCoders.Core.IO;

/// <summary>
/// Deletes a file when disposed, providing automatic cleanup of temporary files.
/// </summary>
/// <param name="fileName">The path of the file to delete on disposal.</param>
/// <param name="throwException">
/// <see langword="true"/> to rethrow exceptions that occur during deletion; <see langword="false"/> to suppress them.
/// </param>
public sealed class FileCleanUp(string fileName, bool throwException = false) : IDisposable
{
    /// <inheritdoc/>
    public void Dispose()
    {
        try
        {
            if (FileSys.File.Exists(FileName))
            {
                FileSys.File.Delete(FileName);
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
    /// Gets the path of the file that will be deleted on disposal.
    /// </summary>
    public string FileName { get; } = Ensure.IsNotNullOrWhitespace(fileName);
}

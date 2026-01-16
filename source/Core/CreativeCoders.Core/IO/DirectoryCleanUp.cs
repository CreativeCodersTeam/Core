using System;

namespace CreativeCoders.Core.IO;

public sealed class DirectoryCleanUp(string directoryName, bool recursive = true, bool throwException = false)
    : IDisposable
{
    public void Dispose()
    {
        try
        {
            FileSys.Directory.Delete(DirectoryName, recursive);
        }
        catch (Exception)
        {
            if (throwException)
            {
                throw;
            }
        }
    }

    public string DirectoryName { get; } = Ensure.IsNotNullOrWhitespace(directoryName);
}

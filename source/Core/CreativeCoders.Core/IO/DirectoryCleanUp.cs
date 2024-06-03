using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.IO;

[PublicAPI]
public sealed class DirectoryCleanUp(string directoryPath, bool throwException = false) : IDisposable
{
    public void Dispose()
    {
        try
        {
            FileSys.Directory.Delete(DirectoryPath, true);
        }
        catch (Exception)
        {
            if (throwException)
            {
                throw;
            }
        }
    }

    public string DirectoryPath { get; } = directoryPath;
}

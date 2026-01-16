using System;

namespace CreativeCoders.Core.IO;

public sealed class FileCleanUp(string fileName, bool throwException = false) : IDisposable
{
    public void Dispose()
    {
        try
        {
            FileSys.File.Delete(FileName);
        }
        catch (Exception)
        {
            if (throwException)
            {
                throw;
            }
        }
    }

    public string FileName { get; } = Ensure.IsNotNullOrWhitespace(fileName);
}

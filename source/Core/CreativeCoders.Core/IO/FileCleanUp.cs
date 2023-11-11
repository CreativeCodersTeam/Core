using System;

namespace CreativeCoders.Core.IO;

public sealed class FileCleanUp : IDisposable
{
    private readonly bool _throwException;

    public FileCleanUp(string fileName, bool throwException = false)
    {
        FileName = fileName;
        _throwException = throwException;
    }

    public void Dispose()
    {
        try
        {
            FileSys.File.Delete(FileName);
        }
        catch (Exception)
        {
            if (_throwException)
            {
                throw;
            }
        }

    }

    public string FileName { get; }
}

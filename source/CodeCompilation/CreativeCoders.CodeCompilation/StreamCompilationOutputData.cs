using System;
using System.IO;

namespace CreativeCoders.CodeCompilation;

public class StreamCompilationOutputData : ICompilationOutputData, IDisposable
{
    private readonly Stream _stream;

    public StreamCompilationOutputData(Stream stream)
    {
        _stream = stream;
    }

    public Stream GetPeStream()
    {
        return _stream;
    }

    public void Dispose()
    {
        _stream?.Dispose();
    }
}

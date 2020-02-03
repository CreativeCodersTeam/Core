using System;
using System.IO;

namespace CreativeCoders.CodeCompilation
{
    public class StreamCompilationOutput : ICompilationOutput, IDisposable
    {
        private readonly Stream _stream;

        public StreamCompilationOutput(Stream stream)
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
}
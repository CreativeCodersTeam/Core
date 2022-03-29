using System.IO;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Servers.Http;

public class StreamResponseBody : IHttpResponseBody
{
    private readonly Stream _stream;

    private StreamWriter _streamWriter;

    public StreamResponseBody(Stream stream)
    {
        _stream = stream;
    }

    public Task WriteAsync(string content)
    {
        _streamWriter ??= new StreamWriter(_stream);
            
        return _streamWriter.WriteAsync(content);
    }

    public async Task FlushAsync()
    {
        if (_streamWriter != null)
        {
            await _streamWriter.FlushAsync().ConfigureAwait(false);
        }
            
        await _stream.FlushAsync().ConfigureAwait(false);
    }

    public Stream GetStream()
    {
        return _stream;
    }
}
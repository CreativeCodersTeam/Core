using System.IO;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Servers.Http;

public class StreamRequestBody : IHttpRequestBody
{
    private readonly Stream _stream;

    public StreamRequestBody(Stream stream)
    {
        _stream = stream;
    }

    public Task<string> ReadAsStringAsync()
    {
        var streamReader = new StreamReader(_stream);
        return streamReader.ReadToEndAsync();
    }

    public Task<Stream> ReadAsStreamAsync()
    {
        return Task.FromResult(_stream);
    }
}
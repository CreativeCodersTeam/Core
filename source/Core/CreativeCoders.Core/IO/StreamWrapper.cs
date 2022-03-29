using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.IO;

[PublicAPI]
[ExcludeFromCodeCoverage]
public class StreamWrapper : Stream
{
    private readonly Action<bool> _disposeBeforeStreamAction;

    private readonly Action<bool> _disposeAfterStreamAction;

    private readonly Stream _dataStream;

    public StreamWrapper(Stream dataStream)
    {
        Ensure.IsNotNull(dataStream, nameof(dataStream));
            
        _dataStream = dataStream;
    }

    public StreamWrapper(Stream dataStream, Action<bool> disposeBeforeStreamAction, Action<bool> disposeAfterStreamAction) : this(dataStream)
    {
        _disposeBeforeStreamAction = disposeBeforeStreamAction;
        _disposeAfterStreamAction = disposeAfterStreamAction;
    }

    public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
    {
        return _dataStream.BeginRead(buffer, offset, count, callback, state);
    }

    public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
    {
        return _dataStream.BeginWrite(buffer, offset, count, callback, state);
    }

    public override void Close()
    {
        _dataStream.Close();
    }

    public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
    {
        return _dataStream.CopyToAsync(destination, bufferSize, cancellationToken);
    }

    protected override void Dispose(bool disposing)
    {
        _disposeBeforeStreamAction?.Invoke(disposing);

        _dataStream.Dispose();

        _disposeAfterStreamAction?.Invoke(disposing);
    }

    public override int EndRead(IAsyncResult asyncResult)
    {
        return _dataStream.EndRead(asyncResult);
    }

    public override void EndWrite(IAsyncResult asyncResult)
    {
        _dataStream.EndWrite(asyncResult);
    }

    public override Task FlushAsync(CancellationToken cancellationToken)
    {
        return _dataStream.FlushAsync(cancellationToken);
    }

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        return _dataStream.ReadAsync(buffer, offset, count, cancellationToken);
    }

    public override int ReadByte()
    {
        return _dataStream.ReadByte();
    }

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        return _dataStream.WriteAsync(buffer, offset, count, cancellationToken);
    }

    public override void WriteByte(byte value)
    {
        _dataStream.WriteByte(value);
    }
        
    public override bool Equals(object obj)
    {
        return _dataStream.Equals(obj);
    }

    public override int GetHashCode()
    {
        return _dataStream.GetHashCode();
    }

    public override string ToString()
    {
        return _dataStream.ToString();
    }

    public override void Flush()
    {
        _dataStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _dataStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _dataStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        _dataStream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _dataStream.Write(buffer, offset, count);
    }

    public override bool CanRead => _dataStream.CanRead;

    public override bool CanSeek => _dataStream.CanSeek;

    public override bool CanWrite => _dataStream.CanWrite;

    public override long Length => _dataStream.Length;

    public override long Position
    {
        get => _dataStream.Position;
        set => _dataStream.Position = value;
    }

    public override bool CanTimeout => _dataStream.CanTimeout;

    public override int ReadTimeout
    {
        get => _dataStream.ReadTimeout;
        set => _dataStream.ReadTimeout = value;
    }

    public override int WriteTimeout
    {
        get => _dataStream.WriteTimeout;
        set => _dataStream.WriteTimeout = value;
    }
}
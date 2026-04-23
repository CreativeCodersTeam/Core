using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.IO;

/// <summary>
/// Wraps a <see cref="Stream"/> and delegates all operations to the underlying stream,
/// allowing custom actions to be executed before and after disposal.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public sealed class StreamWrapper : Stream
{
    private readonly Stream _dataStream;

    private readonly Action<bool> _disposeAfterStreamAction;
    private readonly Action<bool> _disposeBeforeStreamAction;

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamWrapper"/> class with no disposal actions.
    /// </summary>
    /// <param name="dataStream">The underlying stream to wrap.</param>
    public StreamWrapper(Stream dataStream)
        : this(dataStream, NullAction<bool>.Instance,
            NullAction<bool>.Instance) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamWrapper"/> class with custom disposal actions.
    /// </summary>
    /// <param name="dataStream">The underlying stream to wrap.</param>
    /// <param name="disposeBeforeStreamAction">The action to invoke before disposing the underlying stream.</param>
    /// <param name="disposeAfterStreamAction">The action to invoke after disposing the underlying stream.</param>
    public StreamWrapper(Stream dataStream, Action<bool> disposeBeforeStreamAction,
        Action<bool> disposeAfterStreamAction)
    {
        _dataStream = Ensure.NotNull(dataStream);

        _disposeBeforeStreamAction = disposeBeforeStreamAction;
        _disposeAfterStreamAction = disposeAfterStreamAction;
    }

    /// <inheritdoc/>
    public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback,
        object? state)
    {
        return _dataStream.BeginRead(buffer, offset, count, callback, state);
    }

    /// <inheritdoc/>
    public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback,
        object? state)
    {
        return _dataStream.BeginWrite(buffer, offset, count, callback, state);
    }

    /// <inheritdoc/>
    public override void Close()
    {
        _dataStream.Close();
    }

    /// <inheritdoc/>
    public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
    {
        return _dataStream.CopyToAsync(destination, bufferSize, cancellationToken);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        _disposeBeforeStreamAction.Invoke(disposing);

        Dispose();
        _dataStream.Dispose();

        _disposeAfterStreamAction.Invoke(disposing);
    }

    /// <inheritdoc/>
    public override int EndRead(IAsyncResult asyncResult)
    {
        return _dataStream.EndRead(asyncResult);
    }

    /// <inheritdoc/>
    public override void EndWrite(IAsyncResult asyncResult)
    {
        _dataStream.EndWrite(asyncResult);
    }

    /// <inheritdoc/>
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
        return _dataStream.FlushAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count,
        CancellationToken cancellationToken)
    {
        return _dataStream.ReadAsync(buffer, offset, count, cancellationToken);
    }

    /// <inheritdoc/>
    public override ValueTask<int> ReadAsync(Memory<byte> buffer,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _dataStream.ReadAsync(buffer, cancellationToken);
    }

    /// <inheritdoc/>
    public override int ReadByte()
    {
        return _dataStream.ReadByte();
    }

    /// <inheritdoc/>
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        return _dataStream.WriteAsync(buffer, offset, count, cancellationToken);
    }

    /// <inheritdoc/>
    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _dataStream.WriteAsync(buffer, cancellationToken);
    }

    /// <inheritdoc/>
    public override void WriteByte(byte value)
    {
        _dataStream.WriteByte(value);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return _dataStream.Equals(obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return _dataStream.GetHashCode();
    }

    /// <inheritdoc/>
    public override string? ToString()
    {
        return _dataStream.ToString();
    }

    /// <inheritdoc/>
    public override void Flush()
    {
        _dataStream.Flush();
    }

    /// <inheritdoc/>
    public override int Read(byte[] buffer, int offset, int count)
    {
        return _dataStream.Read(buffer, offset, count);
    }

    /// <inheritdoc/>
    public override int Read(Span<byte> buffer)
    {
        return _dataStream.Read(buffer);
    }

    /// <inheritdoc/>
    public override long Seek(long offset, SeekOrigin origin)
    {
        return _dataStream.Seek(offset, origin);
    }

    /// <inheritdoc/>
    public override void SetLength(long value)
    {
        _dataStream.SetLength(value);
    }

    /// <inheritdoc/>
    public override void Write(byte[] buffer, int offset, int count)
    {
        _dataStream.Write(buffer, offset, count);
    }

    /// <inheritdoc/>
    public override bool CanRead => _dataStream.CanRead;

    /// <inheritdoc/>
    public override bool CanSeek => _dataStream.CanSeek;

    /// <inheritdoc/>
    public override bool CanWrite => _dataStream.CanWrite;

    /// <inheritdoc/>
    public override long Length => _dataStream.Length;

    /// <inheritdoc/>
    public override long Position
    {
        get => _dataStream.Position;
        set => _dataStream.Position = value;
    }

    /// <inheritdoc/>
    public override bool CanTimeout => _dataStream.CanTimeout;

    /// <inheritdoc/>
    public override int ReadTimeout
    {
        get => _dataStream.ReadTimeout;
        set => _dataStream.ReadTimeout = value;
    }

    /// <inheritdoc/>
    public override int WriteTimeout
    {
        get => _dataStream.WriteTimeout;
        set => _dataStream.WriteTimeout = value;
    }
}

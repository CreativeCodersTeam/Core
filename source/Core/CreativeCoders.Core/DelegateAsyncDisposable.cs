using System;
using System.Threading.Tasks;

namespace CreativeCoders.Core;

/// <summary>
/// Wraps a delegate as an <see cref="IAsyncDisposable"/> implementation.
/// </summary>
public sealed class DelegateAsyncDisposable : IAsyncDisposable
{
    private readonly Func<ValueTask> _dispose;

    private readonly bool _onlyDisposeOnce;

    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateAsyncDisposable"/> class.
    /// </summary>
    /// <param name="dispose">The function to invoke on asynchronous disposal.</param>
    /// <param name="onlyDisposeOnce">
    /// <see langword="true"/> to invoke the dispose function only once; otherwise, <see langword="false"/>.
    /// </param>
    public DelegateAsyncDisposable(Func<ValueTask> dispose, bool onlyDisposeOnce)
    {
        Ensure.IsNotNull(dispose);

        _dispose = dispose;
        _onlyDisposeOnce = onlyDisposeOnce;
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        if (_onlyDisposeOnce && _isDisposed)
        {
            return ValueTask.CompletedTask;
        }

        _isDisposed = true;

        return _dispose();
    }
}

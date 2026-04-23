using System;

#nullable enable

namespace CreativeCoders.Core;

/// <summary>
/// Wraps a delegate as an <see cref="IDisposable"/> implementation.
/// </summary>
public sealed class DelegateDisposable : IDisposable
{
    private readonly Action _dispose;

    private readonly bool _onlyDisposeOnce;

    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateDisposable"/> class.
    /// </summary>
    /// <param name="dispose">The action to invoke on disposal.</param>
    /// <param name="onlyDisposeOnce">
    /// <see langword="true"/> to invoke the dispose action only once; otherwise, <see langword="false"/>.
    /// </param>
    public DelegateDisposable(Action dispose, bool onlyDisposeOnce)
    {
        Ensure.IsNotNull(dispose);

        _dispose = dispose;
        _onlyDisposeOnce = onlyDisposeOnce;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_onlyDisposeOnce && _isDisposed)
        {
            return;
        }

        _isDisposed = true;

        _dispose();
    }
}

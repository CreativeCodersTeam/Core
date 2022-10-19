using System;

#nullable enable

namespace CreativeCoders.Core;

public sealed class DelegateDisposable : IDisposable
{
    private readonly Action _dispose;

    private readonly bool _onlyDisposeOnce;

    private bool _isDisposed;

    public DelegateDisposable(Action dispose, bool onlyDisposeOnce)
    {
        Ensure.IsNotNull(dispose, nameof(dispose));

        _dispose = dispose;
        _onlyDisposeOnce = onlyDisposeOnce;
    }

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

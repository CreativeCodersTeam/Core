using System;
using System.Threading.Tasks;

namespace CreativeCoders.Core;

public sealed class DelegateAsyncDisposable : IAsyncDisposable
{
    private readonly Func<ValueTask> _dispose;

    private readonly bool _onlyDisposeOnce;

    private bool _isDisposed;

    public DelegateAsyncDisposable(Func<ValueTask> dispose, bool onlyDisposeOnce)
    {
        Ensure.IsNotNull(dispose, nameof(dispose));

        _dispose = dispose;
        _onlyDisposeOnce = onlyDisposeOnce;
    }

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

using System;
using System.Threading.Tasks;
using CreativeCoders.Core.Weak;

namespace CreativeCoders.Messaging.DefaultMediator;

internal class AsyncMediatorRegistration<T> : IMediatorRegistration
{
    private readonly WeakFunc<T, Task> _weakAsyncAction;

    public AsyncMediatorRegistration(object target, Func<T, Task> asyncAction)
    {
        Target = target;
        _weakAsyncAction = new WeakFunc<T, Task>(target, asyncAction);
    }

    public Task ExecuteAsync(object message) => _weakAsyncAction.Execute((T) message);

    public bool IsAlive() => _weakAsyncAction.IsAlive();

    public void Dispose() => _weakAsyncAction?.Dispose();

    public object Target { get; }
}

using System;
using System.Threading.Tasks;
using CreativeCoders.Core.Weak;

namespace CreativeCoders.Messaging.DefaultMediator
{
    internal class AsyncMediatorRegistration<T> : IMediatorRegistration
    {
        private readonly WeakFunc<T, Task> _weakAsyncAction;

        public AsyncMediatorRegistration(object target, Func<T, Task> asyncAction)
        {
            Target = target;
            _weakAsyncAction = new WeakFunc<T, Task>(target, asyncAction);
        }
        
        public void Execute(object message)
        {
            ExecuteAsync(message).Wait();
        }

        public Task ExecuteAsync(object message)
        {
            return _weakAsyncAction.Execute((T) message);
        }

        public object Target { get; }

        public void Dispose()
        {
            _weakAsyncAction?.Dispose();
        }
    }
}
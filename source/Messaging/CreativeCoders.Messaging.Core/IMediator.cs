using System;
using System.Threading.Tasks;

namespace CreativeCoders.Messaging.Core
{
    public interface IMediator
    {
        IDisposable RegisterHandler<TMessage>(object target, Action<TMessage> action);

        IDisposable RegisterAsyncHandler<TMessage>(object target, Func<TMessage, Task> asyncAction);

        void UnregisterHandler(object target);
        
        void UnregisterHandler<TMessage>(object target);
        
        Task SendAsync<TMessage>(TMessage message);
    }
}
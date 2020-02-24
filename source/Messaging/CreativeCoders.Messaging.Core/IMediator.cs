using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Messaging.Core
{
    [PublicAPI]
    public interface IMediator
    {
        IDisposable RegisterHandler<TMessage>(object target, Action<TMessage> action);

        IDisposable RegisterAsyncHandler<TMessage>(object target, Func<TMessage, Task> asyncAction);

        void UnregisterHandler(object target);
        
        void UnregisterHandler<TMessage>(object target);
        
        Task SendAsync<TMessage>(TMessage message);
    }
}
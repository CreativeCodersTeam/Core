using System;
using System.Threading.Tasks;

namespace CreativeCoders.Messaging.Core
{
    public interface IMediator
    {
        IDisposable RegisterHandler<TMessage>(object target, Action<TMessage> action);

        IDisposable RegisterAsyncHandler<TMessage>(object target, Func<TMessage, Task> asyncAction);

        void Send<TMessage>(TMessage message);

        Task SendAsync<TMessage>(TMessage message);
    }
}
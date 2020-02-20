using System;
using System.Threading.Tasks;
using CreativeCoders.Core.Weak;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Messaging
{
    [PublicAPI]
    public interface IMessenger
    {
        IDisposable Register<TMessage>(object receiver, Action<TMessage> action);
        
        IDisposable RegisterAsyncHandler<TMessage>(object receiver, Func<TMessage, Task> asyncAction);

        IDisposable Register<TMessage>(object receiver, Action<TMessage> action, KeepTargetAliveMode keepTargetAliveMode);
        
        IDisposable RegisterAsyncHandler<TMessage>(object receiver, Func<TMessage, Task> asyncAction, KeepTargetAliveMode keepTargetAliveMode);

        void Unregister(object receiver);

        void Unregister<TMessage>(object receiver);

        void Send<TMessage>(TMessage message);

        Task SendAsync<TMessage>(TMessage message);
    }
}
using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Messaging.Core;

[PublicAPI]
public interface IMediator
{
    IDisposable RegisterHandler<TMessage>(object target, Action<TMessage> action)
        where TMessage : notnull;

    IDisposable RegisterAsyncHandler<TMessage>(object target, Func<TMessage, Task> asyncAction)
        where TMessage : notnull;

    void UnregisterHandler(object target);

    void UnregisterHandler<TMessage>(object target)
        where TMessage : notnull;

    Task SendAsync<TMessage>(TMessage message)
        where TMessage : notnull;
}

using System;
using CreativeCoders.Core.Weak;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Messaging;

[PublicAPI]
public interface IMessenger
{
    IDisposable Register<TMessage>(object receiver, Action<TMessage> action);
        
    IDisposable Register<TMessage>(object receiver, Action<TMessage> action, KeepOwnerAliveMode keepOwnerAliveMode);
        
    void Unregister(object receiver);

    void Unregister<TMessage>(object receiver);

    void Send<TMessage>(TMessage message);
}
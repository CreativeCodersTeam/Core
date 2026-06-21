using System;
using System.Reactive.Concurrency;
using JetBrains.Annotations;

namespace CreativeCoders.Reactive.Messaging;

[PublicAPI]
public interface IMessageTopic
{
    void Publish<TMessage>(TMessage message)
        where TMessage : notnull;

    IObservable<TMessage> Register<TMessage>()
        where TMessage : notnull;

    IObservable<TMessage> Register<TMessage>(IScheduler scheduler)
        where TMessage : notnull;
}

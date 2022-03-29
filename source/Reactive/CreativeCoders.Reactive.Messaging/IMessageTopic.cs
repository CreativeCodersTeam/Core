using System;
using System.Reactive.Concurrency;
using JetBrains.Annotations;

namespace CreativeCoders.Reactive.Messaging;

[PublicAPI]
public interface IMessageTopic
{
    void Publish<TMessage>(TMessage message);

    IObservable<TMessage> Register<TMessage>();

    IObservable<TMessage> Register<TMessage>(IScheduler scheduler);
}
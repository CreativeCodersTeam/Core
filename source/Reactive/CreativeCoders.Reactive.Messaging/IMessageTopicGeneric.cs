using System;
using System.Reactive.Concurrency;
using JetBrains.Annotations;

namespace CreativeCoders.Reactive.Messaging;

[PublicAPI]
public interface IMessageTopic<TMessage>
{
    void Publish(TMessage message);

    IObservable<TMessage> Register();

    IObservable<TRegisterMessage> Register<TRegisterMessage>()
        where TRegisterMessage: TMessage;

    IObservable<TMessage> Register(IScheduler scheduler);

    IObservable<TRegisterMessage> Register<TRegisterMessage>(IScheduler scheduler)
        where TRegisterMessage : TMessage;
}
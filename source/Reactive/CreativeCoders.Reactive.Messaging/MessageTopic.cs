using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CreativeCoders.Core;

namespace CreativeCoders.Reactive.Messaging;

public class MessageTopic : IMessageTopic
{
    private readonly ISubject<object> _messageSubject;

    public MessageTopic()
    {
        var subject = new Subject<object>();

        _messageSubject = Subject.Synchronize(subject);
    }

    public void Publish<TMessage>(TMessage message)
        where TMessage : notnull
    {
        Ensure.IsNotNull(message);

        _messageSubject.OnNext(message);
    }

    public IObservable<TMessage> Register<TMessage>()
        where TMessage : notnull
    {
        var registration = _messageSubject
            .OfType<TMessage>()
            .Publish()
            .RefCount();

        return registration;
    }

    public IObservable<TMessage> Register<TMessage>(IScheduler scheduler)
        where TMessage : notnull
    {
        Ensure.IsNotNull(scheduler);

        return Register<TMessage>().ObserveOn(scheduler);
    }
}

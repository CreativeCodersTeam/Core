using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CreativeCoders.Core;

namespace CreativeCoders.Reactive.Messaging;

public class MessageTopic<TMessage> : IMessageTopic<TMessage>
{
    private readonly ISubject<TMessage> _messageSubject;

    public MessageTopic()
    {
        var subject = new Subject<TMessage>();

        _messageSubject = Subject.Synchronize(subject);
    }

    public void Publish(TMessage message)
    {
        Ensure.IsNotNull(message, nameof(message));

        _messageSubject.OnNext(message);
    }

    public IObservable<TMessage> Register()
    {
        return _messageSubject
            .Publish()
            .RefCount();
    }

    public IObservable<TRegisterMessage> Register<TRegisterMessage>()
        where TRegisterMessage : TMessage
    {
        var registration = _messageSubject
            .Where(item => item is TRegisterMessage)
            .Select(item => (TRegisterMessage) item)
            .Publish()
            .RefCount();

        return registration;
    }

    public IObservable<TMessage> Register(IScheduler scheduler)
    {
        Ensure.IsNotNull(scheduler, nameof(scheduler));

        return Register().ObserveOn(scheduler);
    }

    public IObservable<TRegisterMessage> Register<TRegisterMessage>(IScheduler scheduler)
        where TRegisterMessage : TMessage
    {
        Ensure.IsNotNull(scheduler, nameof(scheduler));

        return Register<TRegisterMessage>().ObserveOn(scheduler);
    }
}

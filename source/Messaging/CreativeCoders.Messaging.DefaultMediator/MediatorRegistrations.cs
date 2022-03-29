using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Threading;

namespace CreativeCoders.Messaging.DefaultMediator;

internal class MediatorRegistrations
{
    private readonly ConcurrentDictionary<Type, IList<IMediatorRegistration>> _registrations;

    public MediatorRegistrations()
    {
        _registrations = new ConcurrentDictionary<Type, IList<IMediatorRegistration>>();
    }

    public IDisposable RegisterHandler<TMessage>(object target, Action<TMessage> action)
    {
        return RegisterAsyncHandler<TMessage>(target, message =>
        {
            action(message);
            return Task.CompletedTask;
        });
    }

    public IDisposable RegisterAsyncHandler<TMessage>(object target, Func<TMessage, Task> asyncAction)
    {
        var registration = new AsyncMediatorRegistration<TMessage>(target, asyncAction);

        var typeRegistrations = GetRegistrationList<TMessage>();
        typeRegistrations.Add(registration);

        return new DelegateDisposable(
            () =>
            {
                var registrations = GetRegistrationList<TMessage>();
                registrations.Remove(registration);
                registration.Dispose();
            },
            true);
    }

    private IList<IMediatorRegistration> GetRegistrationList<TMessage>()
    {
        var typeRegistrations =
            _registrations.GetOrAdd(typeof(TMessage), _ => new ConcurrentList<IMediatorRegistration>());
        typeRegistrations.Remove(registration => !registration.IsAlive());

        return typeRegistrations;
    }

    public IEnumerable<IMediatorRegistration> GetRegistrationsForMessage<TMessage>()
    {
        return GetRegistrationList<TMessage>();
    }

    public void UnregisterHandler(object target)
    {
        foreach (var typeRegistrations in _registrations)
        {
            typeRegistrations.Value.Remove(registration => registration.Target == target);
        }
    }

    public void UnregisterHandler<TMessage>(object target)
    {
        var typeRegistrations = GetRegistrationList<TMessage>();

        typeRegistrations.Remove(registration => registration.Target == target);
    }
}

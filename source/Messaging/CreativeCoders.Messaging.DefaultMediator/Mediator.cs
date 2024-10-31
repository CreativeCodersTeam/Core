using System;
using System.Threading.Tasks;
using CreativeCoders.Messaging.Core;

namespace CreativeCoders.Messaging.DefaultMediator;

public class Mediator : IMediator
{
    private readonly MediatorRegistrations _registrations = new MediatorRegistrations();

    public IDisposable RegisterHandler<TMessage>(object target, Action<TMessage> action)
        => _registrations.RegisterHandler(target, action);

    public IDisposable RegisterAsyncHandler<TMessage>(object target, Func<TMessage, Task> asyncAction)
        => _registrations.RegisterAsyncHandler(target, asyncAction);

    public void UnregisterHandler(object target) => _registrations.UnregisterHandler(target);

    public void UnregisterHandler<TMessage>(object target) =>
        _registrations.UnregisterHandler<TMessage>(target);

    public async Task SendAsync<TMessage>(TMessage message)
    {
        var registrationsForMessage = _registrations.GetRegistrationsForMessage<TMessage>();

        foreach (var registration in registrationsForMessage)
        {
            await registration.ExecuteAsync(message).ConfigureAwait(false);
        }
    }
}

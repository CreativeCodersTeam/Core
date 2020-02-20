using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Threading;

namespace CreativeCoders.Messaging.DefaultMediator
{
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

        public IDisposable RegisterAsyncHandler<TMessage>(object target, Func<TMessage,Task> asyncAction)
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
            return _registrations.GetOrAdd(typeof(TMessage), type => new ConcurrentList<IMediatorRegistration>());
        }

        public IEnumerable<IMediatorRegistration> GetRegistrationsForMessage<TMessage>()
        {
            return GetRegistrationList<TMessage>();
        }
    }
}
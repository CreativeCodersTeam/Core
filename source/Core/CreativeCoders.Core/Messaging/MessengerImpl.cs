using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core.Threading;
using CreativeCoders.Core.Weak;

namespace CreativeCoders.Core.Messaging
{
    internal class MessengerImpl : IMessenger
    {
        private readonly ConcurrentDictionary<Type, IList<IMessengerRegistration>> _registrationsForMessages;

        public MessengerImpl()
        {
            _registrationsForMessages = new ConcurrentDictionary<Type, IList<IMessengerRegistration>>();
        }

        public IDisposable Register<TMessage>(object receiver, Action<TMessage> action)
        {
            return Register(receiver, action, KeepTargetAliveMode.NotKeepAlive);
        }

        public IDisposable RegisterAsyncHandler<TMessage>(object receiver, Func<TMessage, Task> asyncAction)
        {
            throw new NotImplementedException();
        }

        public IDisposable Register<TMessage>(object receiver, Action<TMessage> action, KeepTargetAliveMode keepTargetAliveMode)
        {
            var actions = GetRegistrationsForMessageType<TMessage>();
            var registration = new MessengerRegistration<TMessage>(this, receiver, action, keepTargetAliveMode);
            actions.Add(registration);
            return registration;
        }

        public IDisposable RegisterAsyncHandler<TMessage>(object receiver, Func<TMessage, Task> asyncAction, KeepTargetAliveMode keepTargetAliveMode)
        {
            throw new NotImplementedException();
        }

        public void Unregister(object receiver)
        {
            RemoveRegistrations(x => x.Target == receiver);
        }

        public void Unregister<TMessage>(object receiver)
        {
            var registrations = GetRegistrationsForMessageType<TMessage>();
            var registrationsForRemove = registrations.Where(x => x.Target == receiver).ToList();
            foreach (var registration in registrationsForRemove)
            {
                registration.RemovedFromMessenger();
                registrations.Remove(registration);
            }
        }

        internal void RemoveRegistration(IMessengerRegistration registration)
        {
            _registrationsForMessages.Values.ForEach(registrations => registrations.Remove(registration));
        }

        public void Send<TMessage>(TMessage message)
        {
            Ensure.IsNotNull(message, nameof(message));

            var actions = GetRegistrationsForMessageType<TMessage>();
            
            actions.ForEach(action => action.Execute(message));
        }

        public Task SendAsync<TMessage>(TMessage message)
        {
            Ensure.IsNotNull(message, nameof(message));

            var actions = GetRegistrationsForMessageType<TMessage>();
            
            actions.ForEach(action => action.Execute(message));
            
            return Task.CompletedTask;
        }

        private IList<IMessengerRegistration> GetRegistrationsForMessageType<TMessage>()
        {
            CleanupDeadActions();
            var messageType = typeof (TMessage);
            if (!_registrationsForMessages.ContainsKey(messageType))
            {
                _registrationsForMessages[messageType] = new ConcurrentList<IMessengerRegistration>();
            }
            return _registrationsForMessages[messageType];
        }

        private void CleanupDeadActions()
        {
            RemoveRegistrations(x => !x.IsAlive);
        }

        private void RemoveRegistrations(Predicate<IMessengerRegistration> predicate)
        {
            foreach (var receiver in _registrationsForMessages)
            {
                var registrations = receiver.Value;
                var registrationsForRemove = registrations.Where(x => predicate(x)).ToList();
                foreach (var registration in registrationsForRemove)
                {
                    registration.RemovedFromMessenger();
                    registrations.Remove(registration);
                }
            }
        }
    }
}
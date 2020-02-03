using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core.Logging;
using CreativeCoders.Core.Threading;
using CreativeCoders.Core.Weak;

namespace CreativeCoders.Core.Messaging
{
    internal class MessengerImpl : IMessenger
    {
        private static readonly ILogger Log = LogManager.GetLogger<MessengerImpl>();

        private readonly ConcurrentDictionary<Type, IList<IMessengerRegistration>> _registrationsForMessages;

        public MessengerImpl()
        {
            _registrationsForMessages = new ConcurrentDictionary<Type, IList<IMessengerRegistration>>();
        }

        public IDisposable Register<TMessage>(object receiver, Action<TMessage> action)
        {
            return Register(receiver, action, KeepActionTargetAliveMode.NotKeepAlive);
        }

        public IDisposable Register<TMessage>(object receiver, Action<TMessage> action, KeepActionTargetAliveMode keepActionTargetAliveMode)
        {
            var actions = GetRegistrationsForMessageType<TMessage>();
            var registration = new MessengerRegistration<TMessage>(this, receiver, action, keepActionTargetAliveMode);
            actions.Add(registration);
            return registration;
        }

        public void Unregister(object receiver)
        {
            RemoveRegistrations(x => x.Target == receiver);
        }

        public void Unregister<TMessage>(object receiver)
        {
            var registrations = GetRegistrationsForMessageType<TMessage>();
            var registrationsForRemove = registrations.Where(x => x.MessengerAction.Target == receiver).ToList();
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
            foreach (var action in actions.Select(registration => registration.MessengerAction).OfType<WeakAction<TMessage>>())
            {
                ExecuteAction(action, message);
            }
        }

        private static void ExecuteAction<TMessage>(IExecutable<TMessage> action, TMessage message)
        {
            try
            {
                action.Execute(message);
            }
            catch (Exception ex)
            {
                Log.Error("Sending message via action to subscriber failed!", ex);
            }
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

        private void RemoveRegistrations(Predicate<WeakActionBase> predicate)
        {
            foreach (var receiver in _registrationsForMessages)
            {
                var registrations = receiver.Value;
                var registrationsForRemove = registrations.Where(x => predicate(x.MessengerAction)).ToList();
                foreach (var registration in registrationsForRemove)
                {
                    registration.RemovedFromMessenger();
                    registrations.Remove(registration);
                }
            }
        }
    }
}
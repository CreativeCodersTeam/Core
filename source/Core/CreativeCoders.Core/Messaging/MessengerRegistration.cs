using System;
using CreativeCoders.Core.Weak;

namespace CreativeCoders.Core.Messaging
{
    internal class MessengerRegistration<TMessage> : IMessengerRegistration
    {
        private MessengerImpl _messenger;

        private bool _disposed;

        public MessengerRegistration(MessengerImpl messenger, object receiver, Action<TMessage> action,
            KeepActionTargetAliveMode keepActionTargetAliveMode)
        {
            _messenger = messenger;
            MessengerAction = new WeakAction<TMessage>(receiver, action, keepActionTargetAliveMode);
        }

        public WeakActionBase MessengerAction { get; }


        private void Dispose(bool disposing)
        {
            if (!disposing || _disposed)
            {
                return;
            }

            _disposed = true;
            MessengerAction.Dispose();
            _messenger.RemoveRegistration(this);
            _messenger = null;
        }

        public void Dispose()
        {
            Dispose(true);            
        }

        public void RemovedFromMessenger()
        {
            _messenger = null;
            _disposed = true;
        }
    }
}
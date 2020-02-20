using System;

namespace CreativeCoders.Core.Messaging
{
    public interface IMessengerRegistration : IDisposable
    {
        void Execute<TMessage>(TMessage message);
        
        object Target { get; }
        
        bool IsAlive { get; }

        void RemovedFromMessenger();
    }
}
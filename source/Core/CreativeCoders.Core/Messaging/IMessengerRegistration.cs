using System;
using CreativeCoders.Core.Weak;

namespace CreativeCoders.Core.Messaging
{
    public interface IMessengerRegistration : IDisposable
    {
        WeakActionBase MessengerAction { get; }

        void RemovedFromMessenger();
    }
}
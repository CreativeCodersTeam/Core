using System;
using System.Threading.Tasks;

namespace CreativeCoders.Messaging.DefaultMediator
{
    internal interface IMediatorRegistration : IDisposable
    {
        Task ExecuteAsync(object message);
        
        object Target { get; }
        
        bool IsAlive();
    }
}
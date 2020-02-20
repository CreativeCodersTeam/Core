using System;
using System.Threading.Tasks;

namespace CreativeCoders.Messaging.DefaultMediator
{
    internal interface IMediatorRegistration : IDisposable
    {
        void Execute(object message);

        Task ExecuteAsync(object message);
        
        object Target { get; }
    }
}
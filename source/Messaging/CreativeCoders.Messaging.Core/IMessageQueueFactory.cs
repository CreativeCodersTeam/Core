using JetBrains.Annotations;

namespace CreativeCoders.Messaging.Core
{
    [PublicAPI]
    public interface IMessageQueueFactory
    {
        IMessageQueue<T> Create<T>(int maxQueueLength);
    }
}
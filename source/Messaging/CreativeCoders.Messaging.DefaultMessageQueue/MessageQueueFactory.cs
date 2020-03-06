using CreativeCoders.Messaging.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Messaging.DefaultMessageQueue
{
    [PublicAPI]
    public class MessageQueueFactory : IMessageQueueFactory
    {
        public IMessageQueue<T> Create<T>(int maxQueueLength)
        {
            return MessageQueue<T>.Create(maxQueueLength);
        }
    }
}
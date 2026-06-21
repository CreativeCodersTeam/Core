using CreativeCoders.Messaging.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Messaging.DefaultMessageQueue;

[PublicAPI]
public class MessageQueueFactory : IMessageQueueFactory
{
    public IMessageQueue<T> Create<T>(int maxQueueLength)
        where T : notnull
    {
        return MessageQueue<T>.Create(maxQueueLength);
    }
}

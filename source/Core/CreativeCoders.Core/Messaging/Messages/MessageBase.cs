namespace CreativeCoders.Core.Messaging.Messages;

/// <summary>
/// Serves as the base class for all messages sent through the <see cref="IMessenger"/> system.
/// </summary>
public class MessageBase
{
    /// <summary>
    /// Gets or sets a value indicating whether the message has been handled by a receiver.
    /// </summary>
    public bool IsHandled { get; set; }
}

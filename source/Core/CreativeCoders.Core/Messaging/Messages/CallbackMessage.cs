using System;

namespace CreativeCoders.Core.Messaging.Messages;

/// <summary>
/// Represents a message that carries a callback action to be executed by the receiver.
/// </summary>
public class CallbackMessage : MessageBase
{
    private readonly Action _callback;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallbackMessage"/> class.
    /// </summary>
    /// <param name="callback">The action to invoke when the message is executed.</param>
    public CallbackMessage(Action callback)
    {
        _callback = Ensure.NotNull(callback, nameof(callback));
    }

    /// <summary>
    /// Invokes the callback action associated with this message.
    /// </summary>
    public void Execute()
    {
        _callback();
    }
}

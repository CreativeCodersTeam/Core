using System;

namespace CreativeCoders.Core.Messaging.Messages;

public class CallbackMessage : MessageBase
{
    private readonly Action _callback;

    public CallbackMessage(Action callback)
    {
        _callback = Ensure.NotNull(callback, nameof(callback));
    }

    public void Execute()
    {
        _callback();
    }
}

using System;

namespace CreativeCoders.Core.Messaging.Messages;

public class CallbackMessage : MessageBase
{
    private readonly Action _callback;

    public CallbackMessage(Action callback)
    {
        Ensure.IsNotNull(callback, "callback");
        _callback = callback;
    }

    public void Execute()
    {
        _callback();
    }
}

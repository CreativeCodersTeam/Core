namespace CreativeCoders.Core.Messaging.Messages;

public class DialogMessage : MessageBase
{
    public DialogMessage(string messageText)
    {
        MessageText = messageText;
    }

    public string MessageText { get; }
}
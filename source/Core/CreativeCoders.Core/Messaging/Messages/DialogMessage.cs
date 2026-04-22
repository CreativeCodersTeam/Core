namespace CreativeCoders.Core.Messaging.Messages;

/// <summary>
/// Represents a message intended to display a dialog to the user.
/// </summary>
public class DialogMessage : MessageBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DialogMessage"/> class.
    /// </summary>
    /// <param name="messageText">The text to display in the dialog.</param>
    public DialogMessage(string messageText)
    {
        MessageText = messageText;
    }

    /// <summary>
    /// Gets the text to display in the dialog.
    /// </summary>
    public string MessageText { get; }
}

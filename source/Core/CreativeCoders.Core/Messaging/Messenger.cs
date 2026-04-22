namespace CreativeCoders.Core.Messaging;

/// <summary>
/// Provides access to a default <see cref="IMessenger"/> instance and a factory method for
/// creating new messenger instances.
/// </summary>
public static class Messenger
{
    /// <summary>
    /// Gets the default shared <see cref="IMessenger"/> instance.
    /// </summary>
    public static IMessenger Default { get; } = new MessengerImpl();

    /// <summary>
    /// Creates a new independent <see cref="IMessenger"/> instance.
    /// </summary>
    /// <returns>A new <see cref="IMessenger"/> instance.</returns>
    public static IMessenger CreateInstance()
    {
        return new MessengerImpl();
    }
}

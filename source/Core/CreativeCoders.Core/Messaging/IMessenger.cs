using System;
using CreativeCoders.Core.Weak;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Messaging;

/// <summary>
/// Provides a publish-subscribe messaging pattern that enables loosely-coupled communication
/// between components without requiring direct references to each other.
/// </summary>
[PublicAPI]
public interface IMessenger
{
    /// <summary>
    /// Registers a receiver to handle messages of the specified type.
    /// </summary>
    /// <typeparam name="TMessage">The type of message to receive.</typeparam>
    /// <param name="receiver">The object that receives the message.</param>
    /// <param name="action">The action to invoke when a message of the specified type is sent.</param>
    /// <returns>A disposable registration that unregisters the receiver when disposed.</returns>
    IDisposable Register<TMessage>(object receiver, Action<TMessage> action);

    /// <summary>
    /// Registers a receiver to handle messages of the specified type, with control over
    /// whether the registration keeps the owner alive.
    /// </summary>
    /// <typeparam name="TMessage">The type of message to receive.</typeparam>
    /// <param name="receiver">The object that receives the message.</param>
    /// <param name="action">The action to invoke when a message of the specified type is sent.</param>
    /// <param name="keepOwnerAliveMode">The mode that controls whether the registration prevents garbage collection of the owner.</param>
    /// <returns>A disposable registration that unregisters the receiver when disposed.</returns>
    IDisposable Register<TMessage>(object receiver, Action<TMessage> action,
        KeepOwnerAliveMode keepOwnerAliveMode);

    /// <summary>
    /// Unregisters a receiver from all message types.
    /// </summary>
    /// <param name="receiver">The object to unregister.</param>
    void Unregister(object receiver);

    /// <summary>
    /// Unregisters a receiver from messages of the specified type.
    /// </summary>
    /// <typeparam name="TMessage">The type of message to unregister from.</typeparam>
    /// <param name="receiver">The object to unregister.</param>
    void Unregister<TMessage>(object receiver);

    /// <summary>
    /// Sends a message to all registered receivers of the specified type.
    /// </summary>
    /// <typeparam name="TMessage">The type of message to send.</typeparam>
    /// <param name="message">The message instance to send.</param>
    void Send<TMessage>(TMessage message);
}

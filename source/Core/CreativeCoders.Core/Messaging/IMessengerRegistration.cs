using System;

namespace CreativeCoders.Core.Messaging;

/// <summary>
/// Represents a single message subscription within an <see cref="IMessenger"/>, tracking
/// the receiver and providing methods to deliver messages or manage the registration lifecycle.
/// </summary>
public interface IMessengerRegistration : IDisposable
{
    /// <summary>
    /// Delivers a message to the registered receiver.
    /// </summary>
    /// <typeparam name="TMessage">The type of message to deliver.</typeparam>
    /// <param name="message">The message instance to deliver.</param>
    void Execute<TMessage>(TMessage message);

    /// <summary>
    /// Gets the object that registered to receive messages.
    /// </summary>
    object Target { get; }

    /// <summary>
    /// Gets a value indicating whether the registration target is still alive and can receive messages.
    /// </summary>
    bool IsAlive { get; }

    /// <summary>
    /// Notifies the registration that it has been removed from the messenger.
    /// </summary>
    void RemovedFromMessenger();
}

using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core;

/// <summary>
/// Represents a method that handles an event with a strongly typed sender and event argument.
/// </summary>
/// <typeparam name="TSender">The type of the event sender.</typeparam>
/// <typeparam name="TEventArg">The type of the event argument.</typeparam>
/// <param name="sender">The source of the event.</param>
/// <param name="e">The event data.</param>
[PublicAPI]
public delegate void EventHandlerEx<in TSender, in TEventArg>(TSender sender, TEventArg e);

/// <summary>
/// Represents a method that handles an event with a strongly typed sender.
/// </summary>
/// <typeparam name="TSender">The type of the event sender.</typeparam>
/// <param name="sender">The source of the event.</param>
[PublicAPI]
public delegate void EventHandlerEx<in TSender>(TSender sender);

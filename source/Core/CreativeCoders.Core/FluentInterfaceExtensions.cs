using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core;

/// <summary>
/// Provides extension methods for building fluent interfaces.
/// </summary>
[ExcludeFromCodeCoverage]
[PublicAPI]
public static class FluentInterfaceExtensions
{
    /// <summary>
    /// Executes the specified action and returns the fluent interface instance for chaining.
    /// </summary>
    /// <typeparam name="TFluent">The type of the fluent interface.</typeparam>
    /// <param name="fluentInterface">The fluent interface instance.</param>
    /// <param name="fluentAction">The action to execute.</param>
    /// <returns>The fluent interface instance.</returns>
    public static TFluent Fluent<TFluent>(this TFluent fluentInterface, Action fluentAction)
    {
        fluentAction();
        return fluentInterface;
    }

    /// <summary>
    /// Executes the specified action and returns the fluent interface instance for chaining.
    /// </summary>
    /// <typeparam name="TFluent">The type of the fluent interface.</typeparam>
    /// <param name="fluentInterface">The fluent interface instance.</param>
    /// <param name="fluentAction">The action to execute.</param>
    /// <returns>The fluent interface instance.</returns>
    public static TFluent Fluent<TFluent>(this TFluent fluentInterface, Action<TFluent> fluentAction)
    {
        fluentAction(fluentInterface);
        return fluentInterface;
    }

    /// <summary>
    /// Executes the specified function and returns the fluent interface instance for chaining.
    /// </summary>
    /// <typeparam name="TFluent">The type of the fluent interface.</typeparam>
    /// <param name="fluentInterface">The fluent interface instance.</param>
    /// <param name="fluentFunction">The function to execute.</param>
    /// <returns>The fluent interface instance.</returns>
    // ReSharper disable once UnusedParameter.Global
    public static TFluent Fluent<TFluent>(this TFluent fluentInterface, Func<TFluent> fluentFunction)
    {
        return fluentFunction();
    }

    /// <summary>
    /// Executes the specified function and returns the fluent interface instance for chaining.
    /// </summary>
    /// <typeparam name="TFluent">The type of the fluent interface.</typeparam>
    /// <param name="fluentInterface">The fluent interface instance.</param>
    /// <param name="fluentFunction">The function to execute.</param>
    /// <returns>The fluent interface instance.</returns>
    public static TFluent Fluent<TFluent>(this TFluent fluentInterface, Func<TFluent, TFluent> fluentFunction)
    {
        return fluentFunction(fluentInterface);
    }

    /// <summary>
    /// Executes the specified action only if the condition is met and returns the fluent interface instance.
    /// </summary>
    /// <typeparam name="TFluent">The type of the fluent interface.</typeparam>
    /// <param name="fluentInterface">The fluent interface instance.</param>
    /// <param name="condition">
    /// <see langword="true"/> to execute the action; otherwise, <see langword="false"/>.
    /// </param>
    /// <param name="fluentAction">The action to execute.</param>
    /// <returns>The fluent interface instance.</returns>
    public static TFluent FluentIf<TFluent>(this TFluent fluentInterface, bool condition, Action fluentAction)
    {
        if (condition)
        {
            fluentAction();
        }

        return fluentInterface;
    }

    /// <summary>
    /// Executes the specified action only if the condition is met and returns the fluent interface instance.
    /// </summary>
    /// <typeparam name="TFluent">The type of the fluent interface.</typeparam>
    /// <param name="fluentInterface">The fluent interface instance.</param>
    /// <param name="condition">
    /// <see langword="true"/> to execute the action; otherwise, <see langword="false"/>.
    /// </param>
    /// <param name="fluentAction">The action to execute.</param>
    /// <returns>The fluent interface instance.</returns>
    public static TFluent FluentIf<TFluent>(this TFluent fluentInterface, bool condition,
        Action<TFluent> fluentAction)
    {
        if (condition)
        {
            fluentAction(fluentInterface);
        }

        return fluentInterface;
    }

    /// <summary>
    /// Executes the specified function only if the condition is met and returns the fluent interface instance.
    /// </summary>
    /// <typeparam name="TFluent">The type of the fluent interface.</typeparam>
    /// <param name="fluentInterface">The fluent interface instance.</param>
    /// <param name="condition">
    /// <see langword="true"/> to execute the function; otherwise, <see langword="false"/>.
    /// </param>
    /// <param name="fluentFunction">The function to execute.</param>
    /// <returns>The fluent interface instance.</returns>
    public static TFluent FluentIf<TFluent>(this TFluent fluentInterface, bool condition,
        Func<TFluent> fluentFunction)
    {
        return condition
            ? fluentFunction()
            : fluentInterface;
    }

    /// <summary>
    /// Executes the specified function only if the condition is met and returns the fluent interface instance.
    /// </summary>
    /// <typeparam name="TFluent">The type of the fluent interface.</typeparam>
    /// <param name="fluentInterface">The fluent interface instance.</param>
    /// <param name="condition">
    /// <see langword="true"/> to execute the function; otherwise, <see langword="false"/>.
    /// </param>
    /// <param name="fluentFunction">The function to execute.</param>
    /// <returns>The fluent interface instance.</returns>
    public static TFluent FluentIf<TFluent>(this TFluent fluentInterface, bool condition,
        Func<TFluent, TFluent> fluentFunction)
    {
        return condition
            ? fluentFunction(fluentInterface)
            : fluentInterface;
    }
}

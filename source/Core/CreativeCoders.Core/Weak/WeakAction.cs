using System;
using CreativeCoders.Core.Executing;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak;

/// <summary>
/// Represents a weak reference wrapper around an <see cref="Action"/> delegate, preventing the delegate's target from being kept alive solely by the reference.
/// </summary>
[PublicAPI]
public class WeakAction : WeakBase<Action>, IExecutable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WeakAction"/> class using the action's target as the owner.
    /// </summary>
    /// <param name="action">The action delegate to wrap.</param>
    public WeakAction(Action action) : this(action?.Target, action) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeakAction"/> class using the action's target as the owner and the specified keep-alive mode.
    /// </summary>
    /// <param name="action">The action delegate to wrap.</param>
    /// <param name="keepOwnerAliveMode">The mode that determines whether the owner is kept alive.</param>
    public WeakAction(Action action, KeepOwnerAliveMode keepOwnerAliveMode)
        : this(action?.Target, action, keepOwnerAliveMode) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeakAction"/> class with an explicit target owner.
    /// </summary>
    /// <param name="target">The object that owns the action.</param>
    /// <param name="action">The action delegate to wrap.</param>
    public WeakAction(object target, Action action)
        : base(target ?? action?.Target, action, GetAliveMode(action)) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeakAction"/> class with an explicit target owner and the specified keep-alive mode.
    /// </summary>
    /// <param name="target">The object that owns the action.</param>
    /// <param name="action">The action delegate to wrap.</param>
    /// <param name="keepOwnerAliveMode">The mode that determines whether the owner is kept alive.</param>
    public WeakAction(object target, Action action, KeepOwnerAliveMode keepOwnerAliveMode)
        : base(target ?? action?.Target, action, keepOwnerAliveMode) { }

    private static KeepOwnerAliveMode GetAliveMode(Action action)
    {
        return action?.Method.IsStatic == true
            ? KeepOwnerAliveMode.KeepAlive
            : KeepOwnerAliveMode.AutoGuess;
    }

    /// <inheritdoc/>
    public void Execute()
    {
        var action = GetData();
        action?.Invoke();
    }
}

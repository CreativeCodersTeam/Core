using System;
using CreativeCoders.Core.Executing;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak;

/// <summary>
/// Represents a weak reference wrapper around an <see cref="Action{T}"/> delegate, preventing the delegate's target from being kept alive solely by the reference.
/// </summary>
/// <typeparam name="T">The type of the parameter passed to the action.</typeparam>
[PublicAPI]
public class WeakAction<T> : WeakBase<Action<T>>, IExecutable, IExecutable<T>, IExecutableWithParameter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WeakAction{T}"/> class using the action's target as the owner.
    /// </summary>
    /// <param name="action">The action delegate to wrap.</param>
    public WeakAction(Action<T> action) : this(action?.Target, action) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeakAction{T}"/> class using the action's target as the owner and the specified keep-alive mode.
    /// </summary>
    /// <param name="action">The action delegate to wrap.</param>
    /// <param name="keepOwnerAliveMode">The mode that determines whether the owner is kept alive.</param>
    public WeakAction(Action<T> action, KeepOwnerAliveMode keepOwnerAliveMode)
        : this(action?.Target, action, keepOwnerAliveMode) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeakAction{T}"/> class with an explicit target owner.
    /// </summary>
    /// <param name="target">The object that owns the action.</param>
    /// <param name="action">The action delegate to wrap.</param>
    public WeakAction(object target, Action<T> action)
        : base(target ?? action?.Target, action, KeepOwnerAliveMode.NotKeepAlive) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeakAction{T}"/> class with an explicit target owner and the specified keep-alive mode.
    /// </summary>
    /// <param name="target">The object that owns the action.</param>
    /// <param name="action">The action delegate to wrap.</param>
    /// <param name="keepOwnerAliveMode">The mode that determines whether the owner is kept alive.</param>
    public WeakAction(object target, Action<T> action, KeepOwnerAliveMode keepOwnerAliveMode)
        : base(target ?? action?.Target, action, keepOwnerAliveMode) { }

    /// <inheritdoc/>
    public void Execute()
    {
        Execute(default);
    }

    /// <inheritdoc/>
    public void Execute(T parameter)
    {
        var action = GetData();
        action?.Invoke(parameter);
    }

    /// <inheritdoc/>
    public void Execute(object parameter)
    {
        Execute((T) parameter);
    }
}

using System;
using CreativeCoders.Core.Executing;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak;

/// <summary>
/// Represents a weak reference wrapper around a <see cref="Func{TResult}"/> delegate, preventing the delegate's target from being kept alive solely by the reference.
/// </summary>
/// <typeparam name="T">The type of the return value of the function.</typeparam>
[PublicAPI]
public class WeakFunc<T> : WeakBase<Func<T>>, IExecutableWithResult<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WeakFunc{T}"/> class using the function's target as the owner.
    /// </summary>
    /// <param name="function">The function delegate to wrap.</param>
    public WeakFunc(Func<T> function) : this(function?.Target, function) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeakFunc{T}"/> class using the function's target as the owner and the specified keep-alive mode.
    /// </summary>
    /// <param name="function">The function delegate to wrap.</param>
    /// <param name="keepOwnerAliveMode">The mode that determines whether the owner is kept alive.</param>
    public WeakFunc(Func<T> function, KeepOwnerAliveMode keepOwnerAliveMode)
        : this(function?.Target, function, keepOwnerAliveMode) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeakFunc{T}"/> class with an explicit target owner.
    /// </summary>
    /// <param name="target">The object that owns the function.</param>
    /// <param name="function">The function delegate to wrap.</param>
    public WeakFunc(object target, Func<T> function)
        : this(target, function, KeepOwnerAliveMode.NotKeepAlive) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeakFunc{T}"/> class with an explicit target owner and the specified keep-alive mode.
    /// </summary>
    /// <param name="target">The object that owns the function.</param>
    /// <param name="function">The function delegate to wrap.</param>
    /// <param name="keepOwnerAliveMode">The mode that determines whether the owner is kept alive.</param>
    public WeakFunc(object target, Func<T> function, KeepOwnerAliveMode keepOwnerAliveMode)
        : base(target ?? function?.Target, function, keepOwnerAliveMode) { }

    /// <inheritdoc/>
    public T Execute()
    {
        var function = GetData();
        return function();
    }
}

/// <summary>
/// Represents a weak reference wrapper around a <see cref="Func{T, TResult}"/> delegate, preventing the delegate's target from being kept alive solely by the reference.
/// </summary>
/// <typeparam name="TParameter">The type of the parameter passed to the function.</typeparam>
/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
[PublicAPI]
public class WeakFunc<TParameter, TResult> : WeakBase<Func<TParameter, TResult>>,
    IExecutableWithResult<TParameter, TResult>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WeakFunc{TParameter, TResult}"/> class using the function's target as the owner.
    /// </summary>
    /// <param name="function">The function delegate to wrap.</param>
    public WeakFunc(Func<TParameter, TResult> function) : this(function?.Target, function) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeakFunc{TParameter, TResult}"/> class using the function's target as the owner and the specified keep-alive mode.
    /// </summary>
    /// <param name="function">The function delegate to wrap.</param>
    /// <param name="keepOwnerAliveMode">The mode that determines whether the owner is kept alive.</param>
    public WeakFunc(Func<TParameter, TResult> function, KeepOwnerAliveMode keepOwnerAliveMode)
        : this(function?.Target, function, keepOwnerAliveMode) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeakFunc{TParameter, TResult}"/> class with an explicit target owner.
    /// </summary>
    /// <param name="target">The object that owns the function.</param>
    /// <param name="function">The function delegate to wrap.</param>
    public WeakFunc(object target, Func<TParameter, TResult> function)
        : base(target ?? function?.Target, function, KeepOwnerAliveMode.NotKeepAlive) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeakFunc{TParameter, TResult}"/> class with an explicit target owner and the specified keep-alive mode.
    /// </summary>
    /// <param name="target">The object that owns the function.</param>
    /// <param name="function">The function delegate to wrap.</param>
    /// <param name="keepOwnerAliveMode">The mode that determines whether the owner is kept alive.</param>
    public WeakFunc(object target, Func<TParameter, TResult> function, KeepOwnerAliveMode keepOwnerAliveMode)
        : base(target ?? function?.Target, function, keepOwnerAliveMode) { }

    /// <inheritdoc/>
    public TResult Execute(TParameter parameter)
    {
        var function = GetData();
        return function(parameter);
    }
}

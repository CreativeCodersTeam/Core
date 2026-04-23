using System;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Provides factory methods for creating <see cref="SynchronizedValue{T}"/> instances.
/// </summary>
[PublicAPI]
public static class SynchronizedValue
{
    /// <summary>
    ///     Creates a new <see cref="SynchronizedValue{T}"/> with the default value for the specified value type.
    /// </summary>
    /// <typeparam name="T">The type of the value. Must be a value type.</typeparam>
    /// <returns>A new <see cref="SynchronizedValue{T}"/> initialized to the default value.</returns>
    public static SynchronizedValue<T> Create<T>()
        where T : struct
    {
        return new SynchronizedValue<T>(default);
    }

    /// <summary>
    ///     Creates a new <see cref="SynchronizedValue{T}"/> with the specified initial value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The initial value.</param>
    /// <returns>A new <see cref="SynchronizedValue{T}"/> initialized to the specified value.</returns>
    public static SynchronizedValue<T> Create<T>(T value)
    {
        return new SynchronizedValue<T>(value);
    }

    /// <summary>
    ///     Creates a new <see cref="SynchronizedValue{T}"/> with the default value for the specified value type
    ///     and the specified locking mechanism.
    /// </summary>
    /// <typeparam name="T">The type of the value. Must be a value type.</typeparam>
    /// <param name="lockingMechanism">The locking mechanism used to synchronize access.</param>
    /// <returns>A new <see cref="SynchronizedValue{T}"/> initialized to the default value.</returns>
    public static SynchronizedValue<T> Create<T>(ILockingMechanism lockingMechanism)
        where T : struct
    {
        return new SynchronizedValue<T>(lockingMechanism, default);
    }

    /// <summary>
    ///     Creates a new <see cref="SynchronizedValue{T}"/> with the specified initial value
    ///     and locking mechanism.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="lockingMechanism">The locking mechanism used to synchronize access.</param>
    /// <param name="value">The initial value.</param>
    /// <returns>A new <see cref="SynchronizedValue{T}"/> initialized to the specified value.</returns>
    public static SynchronizedValue<T> Create<T>(ILockingMechanism lockingMechanism, T value)
    {
        return new SynchronizedValue<T>(lockingMechanism, value);
    }
}

/// <summary>
///     Provides a thread-safe wrapper around a value of type <typeparamref name="T"/>,
///     using an <see cref="ILockingMechanism"/> to synchronize read and write access.
/// </summary>
/// <typeparam name="T">The type of the synchronized value.</typeparam>
[PublicAPI]
public class SynchronizedValue<T>
{
    private readonly ILockingMechanism _lockingMechanism;

    private T _value;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SynchronizedValue{T}"/> class
    ///     with the specified initial value and a default locking mechanism.
    /// </summary>
    /// <param name="value">The initial value.</param>
    internal SynchronizedValue(T value) : this(DefaultLockingMechanism(), value) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SynchronizedValue{T}"/> class
    ///     with the specified locking mechanism and initial value.
    /// </summary>
    /// <param name="lockingMechanism">The locking mechanism used to synchronize access.</param>
    /// <param name="value">The initial value.</param>
    internal SynchronizedValue(ILockingMechanism lockingMechanism, T value)
    {
        Ensure.IsNotNull(lockingMechanism);

        _lockingMechanism = lockingMechanism;
        _value = value;
    }

    private static LockSlimLockingMechanism DefaultLockingMechanism()
    {
        return new LockSlimLockingMechanism();
    }

    /// <summary>
    ///     Sets the value by applying the specified transformation function within a write lock.
    /// </summary>
    /// <param name="setValue">A function that receives the current value and returns the new value.</param>
    public void SetValue(Func<T, T> setValue)
    {
        _lockingMechanism.Write(() => { _value = SetValueCore(setValue); });
    }

    private T SetValueCore(Func<T, T> setValue)
    {
        return setValue(_value);
    }

    /// <summary>
    ///     Gets or sets the synchronized value. Read access uses a read lock; write access uses
    ///     a write lock.
    /// </summary>
    public T Value
    {
        get => _lockingMechanism.Read(() => _value);
        set => SetValue(_ => value);
    }
}

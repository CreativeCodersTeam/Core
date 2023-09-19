using System;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.Threading;

public static class SynchronizedValue
{
    public static SynchronizedValue<T> Create<T>()
        where T : struct
    {
        return new SynchronizedValue<T>(default);
    }

    public static SynchronizedValue<T> Create<T>(T value)
    {
        return new SynchronizedValue<T>(value);
    }

    public static SynchronizedValue<T> Create<T>(ILockingMechanism lockingMechanism)
        where T : struct
    {
        return new SynchronizedValue<T>(lockingMechanism, default);
    }

    public static SynchronizedValue<T> Create<T>(ILockingMechanism lockingMechanism, T value)
    {
        return new SynchronizedValue<T>(lockingMechanism, value);
    }
}

[PublicAPI]
public class SynchronizedValue<T>
{
    private readonly ILockingMechanism _lockingMechanism;

    private T _value;

    internal SynchronizedValue(T value) : this(DefaultLockingMechanism(), value) { }

    internal SynchronizedValue(ILockingMechanism lockingMechanism, T value)
    {
        Ensure.IsNotNull(lockingMechanism, nameof(lockingMechanism));

        _lockingMechanism = lockingMechanism;
        _value = value;
    }

    private static ILockingMechanism DefaultLockingMechanism()
    {
        return new LockSlimLockingMechanism();
    }

    public void SetValue(Func<T, T> setValue)
    {
        _lockingMechanism.Write(() =>
        {
            _value = SetValueCore(setValue);
        });
    }

    private T SetValueCore(Func<T, T> setValue)
    {
        return setValue(_value);
    }

    public T Value
    {
        get => _lockingMechanism.Read(() => _value);
        set => SetValue(_ => value);
    }
}

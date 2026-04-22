using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

/// <summary>Represents an argument that can be validated.</summary>
/// <typeparam name="T">The type of the argument value.</typeparam>
[PublicAPI]
public readonly struct Argument<T>
{
    /// <summary>Initializes a new instance of the <see cref="Argument{T}"/> struct.</summary>
    /// <param name="value">The argument value.</param>
    /// <param name="name">The name of the argument, automatically captured from the caller expression.</param>
    public Argument(T? value,
        [CallerArgumentExpression("value")] string name = "[unknown]")
    {
        Value = value;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary>Determines whether the argument has a non-null value.</summary>
    /// <returns><see langword="true"/> if the value is not <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasValue() => Value is not null;

    /// <summary>Casts the argument value to <typeparamref name="TValue"/>.</summary>
    /// <typeparam name="TValue">The target type to cast the value to.</typeparam>
    /// <returns>The argument value cast to <typeparamref name="TValue"/>, or <see langword="null"/> if the value is <see langword="null"/>.</returns>
    public TValue? Cast<TValue>()
    {
        object? objectValue = Value;

        return (TValue?) objectValue;
    }

    /// <summary>Gets the value of the argument.</summary>
    /// <value>The argument value.</value>
    public T? Value { get; }

    /// <summary>Gets the name of the argument.</summary>
    /// <value>The argument name.</value>
    public string Name { get; }

    /// <summary>Implicitly converts an <see cref="Argument{T}"/> to its underlying value.</summary>
    /// <param name="argument">The argument to convert.</param>
    /// <returns>The value of the argument.</returns>
    public static implicit operator T?(Argument<T> argument)
    {
        return argument.Value;
    }
}

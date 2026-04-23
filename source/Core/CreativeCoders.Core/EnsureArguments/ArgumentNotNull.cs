using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

/// <summary>Represents an argument that is guaranteed to be not <see langword="null"/> and can be validated.</summary>
/// <typeparam name="T">The type of the argument value.</typeparam>
[PublicAPI]
public readonly struct ArgumentNotNull<T>
{
    /// <summary>Initializes a new instance of the <see cref="ArgumentNotNull{T}"/> struct.</summary>
    /// <param name="value">The argument value.</param>
    /// <param name="name">The name of the argument, automatically captured from the caller expression.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    public ArgumentNotNull(T value,
        [CallerArgumentExpression("value")] string name = "[unknown]")
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        Value = value;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary>Determines whether the argument has a non-null value.</summary>
    /// <returns><see langword="true"/> if the value is not <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasValue() => true;

    /// <summary>Casts the argument value to <typeparamref name="TValue"/>.</summary>
    /// <typeparam name="TValue">The target type to cast the value to.</typeparam>
    /// <returns>The argument value cast to <typeparamref name="TValue"/>.</returns>
    public TValue Cast<TValue>()
    {
        object objectValue = Value;

        return (TValue) objectValue;
    }

    /// <summary>Gets the value of the argument.</summary>
    /// <value>The argument value, guaranteed to be not <see langword="null"/>.</value>
    [System.Diagnostics.CodeAnalysis.NotNull]
    public T Value { get; }

    /// <summary>Gets the name of the argument.</summary>
    /// <value>The argument name.</value>
    public string Name { get; }

    /// <summary>Implicitly converts an <see cref="ArgumentNotNull{T}"/> to its underlying value.</summary>
    /// <param name="argument">The argument to convert.</param>
    /// <returns>The value of the argument.</returns>
    public static implicit operator T(ArgumentNotNull<T> argument)
    {
        return argument.Value;
    }
}

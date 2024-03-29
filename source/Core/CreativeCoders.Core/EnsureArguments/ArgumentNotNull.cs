﻿using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

///-------------------------------------------------------------------------------------------------
/// <summary>
///     Represents an argument that is known to be not null, which can be validated.
/// </summary>
///
/// <typeparam name="T">    Generic type parameter. </typeparam>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public readonly struct ArgumentNotNull<T>
{
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

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Query if this object has value. </summary>
    ///
    /// <returns>   True if value is not null, false if not. </returns>
    ///-------------------------------------------------------------------------------------------------
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasValue() => true;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Casts the arguments value to TValue. </summary>
    ///
    /// <typeparam name="TValue">   Type of the value. </typeparam>
    ///
    /// <returns>   The casted argument value </returns>
    ///-------------------------------------------------------------------------------------------------
    public TValue Cast<TValue>()
    {
        object objectValue = Value;

        return (TValue) objectValue;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the value of the argument. </summary>
    ///
    /// <value> The value. </value>
    ///-------------------------------------------------------------------------------------------------
    [System.Diagnostics.CodeAnalysis.NotNull]
    public T Value { get; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the name of the argument. </summary>
    ///
    /// <value> The name. </value>
    ///-------------------------------------------------------------------------------------------------
    public string Name { get; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Implicit cast that converts the given Argument&lt;T&gt; to its value T </summary>
    ///
    /// <param name="argument"> The argument. </param>
    ///
    /// <returns>   The value of the argument. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static implicit operator T(ArgumentNotNull<T> argument)
    {
        return argument.Value;
    }
}

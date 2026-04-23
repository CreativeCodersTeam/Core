using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core;

/// <summary>
/// Provides extension methods for <see cref="object"/>.
/// </summary>
[PublicAPI]
public static class ObjectExtensions
{
    /// <summary>
    /// Converts the object to its string representation, returning the specified default value
    /// if the object is <see langword="null"/>.
    /// </summary>
    /// <param name="obj">The object to convert.</param>
    /// <param name="defaultValue">The default value to return if the object is <see langword="null"/>.</param>
    /// <returns>The string representation of the object, or <paramref name="defaultValue"/> if <see langword="null"/>.</returns>
    public static string ToStringSafe(this object? obj, string defaultValue)
    {
        return obj?.ToString() ?? defaultValue;
    }

    /// <summary>
    /// Converts the object to its string representation, returning an empty string
    /// if the object is <see langword="null"/>.
    /// </summary>
    /// <param name="obj">The object to convert.</param>
    /// <returns>The string representation of the object, or an empty string if <see langword="null"/>.</returns>
    public static string ToStringSafe(this object? obj)
    {
        return obj.ToStringSafe(string.Empty);
    }

    /// <summary>
    /// Attempts to cast the object to the specified type, returning a default value on failure.
    /// </summary>
    /// <typeparam name="T">The target type.</typeparam>
    /// <param name="instance">The object to cast.</param>
    /// <param name="defaultValue">The value to return if the cast fails.</param>
    /// <returns>The cast instance, or <paramref name="defaultValue"/> if the cast fails.</returns>
    public static T? As<T>(this object instance, T? defaultValue)
    {
        if (instance is T value)
        {
            return value;
        }

        return defaultValue;
    }

    /// <summary>
    /// Attempts to cast the object to the specified type, returning the default value on failure.
    /// </summary>
    /// <typeparam name="T">The target type.</typeparam>
    /// <param name="instance">The object to cast.</param>
    /// <returns>The cast instance, or the default value of <typeparamref name="T"/> if the cast fails.</returns>
    public static T? As<T>(this object instance) => As<T>(instance, default);

    /// <summary>
    /// Attempts to cast the object to the specified type.
    /// </summary>
    /// <typeparam name="T">The target type.</typeparam>
    /// <param name="instance">The object to cast.</param>
    /// <param name="asInstance">When this method returns, contains the cast instance if successful. This parameter is treated as uninitialized.</param>
    /// <returns><see langword="true"/> if the cast succeeds; otherwise, <see langword="false"/>.</returns>
    public static bool TryAs<T>(this object instance, [MaybeNullWhen(false)] out T asInstance)
    {
        if (instance is T value)
        {
            asInstance = value;
            return true;
        }

        asInstance = default;
        return false;
    }

    /// <summary>
    /// Casts the object to the specified type.
    /// </summary>
    /// <typeparam name="T">The target type.</typeparam>
    /// <param name="instance">The object to cast.</param>
    /// <returns>The cast instance.</returns>
    /// <exception cref="InvalidCastException">The object cannot be cast to type <typeparamref name="T"/>.</exception>
    public static T CastAs<T>(this object instance) => (T)Ensure.NotNull(instance);

    /// <summary>
    /// Attempts to dispose the object asynchronously if it implements <see cref="IAsyncDisposable"/> or <see cref="IDisposable"/>.
    /// </summary>
    /// <param name="instance">The object to dispose.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    public static async ValueTask TryDisposeAsync(this object instance)
    {
        switch (instance)
        {
            case IAsyncDisposable asyncDisposable:
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                break;
            case IDisposable disposable:
                disposable.Dispose();
                break;
        }
    }

    /// <summary>
    /// Gets the value of the specified property using reflection.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="instance">The object to read the property from.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>The property value.</returns>
    /// <exception cref="MissingMemberException">The property does not exist on the object's type.</exception>
    public static T? GetPropertyValue<T>(this object instance, string propertyName)
    {
        var propInfo = instance.GetType().GetProperty(propertyName);

        if (propInfo == null)
        {
            throw new MissingMemberException(instance.GetType().Name, propertyName);
        }

        return (T?)propInfo.GetValue(instance);
    }

    /// <summary>
    /// Sets the value of the specified property using reflection.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="instance">The object to set the property on.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="value">The value to set.</param>
    /// <exception cref="MissingMemberException">The property does not exist on the object's type.</exception>
    public static void SetPropertyValue<T>(this object instance, string propertyName, T? value)
    {
        var propInfo = instance.GetType().GetProperty(propertyName);

        if (propInfo == null)
        {
            throw new MissingMemberException(instance.GetType().Name, propertyName);
        }

        propInfo.SetValue(instance, value);
    }

    /// <summary>
    /// Converts the object's public instance properties to a dictionary.
    /// </summary>
    /// <param name="obj">The object to convert.</param>
    /// <returns>A dictionary containing the property names and values.</returns>
    public static Dictionary<string, object?> ToDictionary(this object obj)
    {
        Ensure.NotNull(obj);

        return obj
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(propertyInfo => ReadProperty(obj, propertyInfo))
            .ToDictionary(x => x.PropertyName, x => x.PropertyValue);
    }

    private static (string PropertyName, object? PropertyValue) ReadProperty(object obj,
        PropertyInfo propertyInfo)
        => (propertyInfo.Name, propertyInfo.GetValue(obj));
}

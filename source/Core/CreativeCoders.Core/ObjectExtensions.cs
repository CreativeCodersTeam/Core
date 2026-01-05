using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core;

[PublicAPI]
public static class ObjectExtensions
{
    public static string ToStringSafe(this object? obj, string defaultValue)
    {
        return obj?.ToString() ?? defaultValue;
    }

    public static string ToStringSafe(this object? obj)
    {
        return obj.ToStringSafe(string.Empty);
    }

    public static T? As<T>(this object instance, T? defaultValue)
    {
        if (instance is T value)
        {
            return value;
        }

        return defaultValue;
    }

    public static T? As<T>(this object instance) => As<T>(instance, default);

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

    public static T CastAs<T>(this object instance) => (T)Ensure.NotNull(instance);

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

    public static T? GetPropertyValue<T>(this object instance, string propertyName)
    {
        var propInfo = instance.GetType().GetProperty(propertyName);

        if (propInfo == null)
        {
            throw new MissingMemberException(instance.GetType().Name, propertyName);
        }

        return (T?)propInfo.GetValue(instance);
    }

    public static void SetPropertyValue<T>(this object instance, string propertyName, T? value)
    {
        var propInfo = instance.GetType().GetProperty(propertyName);

        if (propInfo == null)
        {
            throw new MissingMemberException(instance.GetType().Name, propertyName);
        }

        propInfo.SetValue(instance, value);
    }

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

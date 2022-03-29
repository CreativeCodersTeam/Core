using System;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

public static class NullArgumentExtensions
{
    public static ArgumentNotNull<T> NotNull<T>(in this Argument<T> argument, string? message = null)
    {
        if (argument.Value is null)
        {
            ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
        }

        return new ArgumentNotNull<T>(argument.Value, argument.Name);
    }

    public static T? Null<T>(in this Argument<T> argument, string? message = null)
    {
        if (argument.Value is not null)
        {
            throw new ArgumentException(message ?? "Argument is not null", argument.Name);
        }

        return argument.Value;
    }
}
using System;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

public static class GuidArgumentExtensions
{
    public static ref readonly Argument<Guid> NotEmpty(in this Argument<Guid> argument,
        string? message = null)
    {
        if (argument.Value == Guid.Empty)
        {
            throw new ArgumentException(message ?? "GUID is empty", argument.Name);
        }

        return ref argument;
    }

    public static ref readonly Argument<Guid?> NotEmpty(in this Argument<Guid?> argument,
        string? message = null)
    {
        if (!argument.Value.HasValue)
        {
            ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
        }

        if (argument.Value.Value == Guid.Empty)
        {
            throw new ArgumentException(message ?? "GUID is empty", argument.Name);
        }

        return ref argument;
    }

    public static ref readonly ArgumentNotNull<Guid> NotEmpty(in this ArgumentNotNull<Guid> argument,
        string? message = null)
    {
        if (argument.Value == Guid.Empty)
        {
            throw new ArgumentException(message ?? "GUID is empty", argument.Name);
        }

        return ref argument;
    }

    public static ref readonly ArgumentNotNull<Guid?> NotEmpty(in this ArgumentNotNull<Guid?> argument,
        string? message = null)
    {
        if (argument.Value == Guid.Empty)
        {
            throw new ArgumentException(message ?? "GUID is empty", argument.Name);
        }

        return ref argument;
    }
}

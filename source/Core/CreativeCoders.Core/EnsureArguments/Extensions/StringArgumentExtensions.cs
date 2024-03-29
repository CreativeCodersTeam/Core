﻿using System;

// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

#nullable enable

public static class StringArgumentExtensions
{
    public static ArgumentNotNull<string> HasMinLength(this Argument<string> argument, uint minLength, string? message = null)
    {
        if (argument.Value is null)
        {
            ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
        }

        if (argument.Value.Length < minLength)
        {
            throw new ArgumentException(message ?? $"Argument '{argument.Name}' has not the minimum length of {minLength}", argument.Name);
        }

        return new ArgumentNotNull<string>(argument.Value, argument.Name);
    }

    public static ref readonly ArgumentNotNull<string> HasMinLength(this in ArgumentNotNull<string> argument, uint minLength, string? message = null)
    {
        if (argument.Value.Length < minLength)
        {
            throw new ArgumentException(message ?? $"Argument '{argument.Name}' has not the minimum length of {minLength}", argument.Name);
        }

        return ref argument;
    }

    public static ArgumentNotNull<string> HasMaxLength(this Argument<string> argument, uint maxLength,
        string? message = null)
    {
        if (argument.Value is null)
        {
            ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
        }

        if (argument.Value.Length > maxLength)
        {
            throw new ArgumentException(message ?? $"Argument '{argument.Name}' has exceeded the maximum length of {maxLength}", argument.Name);
        }

        return new ArgumentNotNull<string>(argument.Value, argument.Name);
    }

    public static ref readonly ArgumentNotNull<string> HasMaxLength(this in ArgumentNotNull<string> argument, uint maxLength, string? message = null)
    {
        if (argument.Value.Length > maxLength)
        {
            throw new ArgumentException(message ?? $"Argument '{argument.Name}' has exceeded the maximum length of {maxLength}", argument.Name);
        }

        return ref argument;
    }
}

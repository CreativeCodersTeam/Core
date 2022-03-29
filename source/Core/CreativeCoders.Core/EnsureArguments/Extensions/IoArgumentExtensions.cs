using System.IO;
using CreativeCoders.Core.IO;

// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

public static class IoArgumentExtensions
{
    public static ArgumentNotNull<string> FileExists(in this Argument<string> argument,
        string message = null)
    {
        if (argument.Value is null)
        {
            ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
        }

        if (!FileSys.File.Exists(argument.Value))
        {
            throw new FileNotFoundException(message ?? $"Argument '{argument.Name}' file not found",
                argument.Value);
        }

        return new ArgumentNotNull<string>(argument.Value, argument.Name);
    }

    public static ref readonly ArgumentNotNull<string> FileExists(
        in this ArgumentNotNull<string> argument, string message = null)
    {
        if (!FileSys.File.Exists(argument.Value))
        {
            throw new FileNotFoundException(message ?? $"Argument '{argument.Name}' file not found",
                argument.Value);
        }

        return ref argument;
    }

    public static ArgumentNotNull<string> DirectoryExists(in this Argument<string> argument,
        string message = null)
    {
        if (argument.Value is null)
        {
            ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
        }

        if (!FileSys.Directory.Exists(argument.Value))
        {
            throw new DirectoryNotFoundException(message ??
                                                 $"Argument '{argument.Name}' directory '{argument.Value}' not found");
        }

        return new ArgumentNotNull<string>(argument.Value, argument.Name);
    }

    public static ref readonly ArgumentNotNull<string> DirectoryExists(
        in this ArgumentNotNull<string> argument, string message = null)
    {
        if (!FileSys.Directory.Exists(argument.Value))
        {
            throw new DirectoryNotFoundException(message ??
                                                 $"Argument '{argument.Name}' directory '{argument.Value}' not found");
        }

        return ref argument;
    }
}
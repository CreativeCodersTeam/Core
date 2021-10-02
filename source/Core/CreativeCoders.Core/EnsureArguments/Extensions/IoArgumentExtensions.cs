using System.IO;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core
{
    public static class IoArgumentExtensions
    {
        public static ref readonly Argument<string> FileExists(in this Argument<string> argument, string? message = null)
        {
            if (argument.Value is null)
            {
                ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
            }

            if (!File.Exists(argument.Value))
            {
                throw new FileNotFoundException(message ?? $"Argument '{argument.Name}' file not found", argument.Value);
            }

            return ref argument;
        }
        
        public static ref readonly ArgumentNotNull<string> FileExists(in this ArgumentNotNull<string> argument, string? message = null)
        {
            if (!File.Exists(argument.Value))
            {
                throw new FileNotFoundException(message ?? $"Argument '{argument.Name}' file not found", argument.Value);
            }

            return ref argument;
        }

        public static ref readonly Argument<string> DirectoryExists(in this Argument<string> argument, string? message = null)
        {
            if (argument.Value is null)
            {
                ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
            }

            if (!Directory.Exists(argument.Value))
            {
                throw new DirectoryNotFoundException(message ?? $"Argument '{argument.Name}' directory '{argument.Value}' not found");
            }

            return ref argument;
        }
        
        public static ref readonly ArgumentNotNull<string> DirectoryExists(in this ArgumentNotNull<string> argument, string? message = null)
        {
            if (!Directory.Exists(argument.Value))
            {
                throw new DirectoryNotFoundException(message ?? $"Argument '{argument.Name}' directory '{argument.Value}' not found");
            }

            return ref argument;
        }
    }
}

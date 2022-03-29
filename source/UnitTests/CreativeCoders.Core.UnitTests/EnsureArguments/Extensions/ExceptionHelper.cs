#nullable enable
namespace CreativeCoders.Core.UnitTests.EnsureArguments.Extensions;

public class ExceptionHelper
{
    public static string GetMessage(string message, string paramName)
        => $"{message} (Parameter '{paramName}')";
}
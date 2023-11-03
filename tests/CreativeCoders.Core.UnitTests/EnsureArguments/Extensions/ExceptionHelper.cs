#nullable enable
using JetBrains.Annotations;

namespace CreativeCoders.Core.UnitTests.EnsureArguments.Extensions;

[UsedImplicitly]
public class ExceptionHelper
{
    public static string GetMessage(string message, string paramName)
        => $"{message} (Parameter '{paramName}')";
}

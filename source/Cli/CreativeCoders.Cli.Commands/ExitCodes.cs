using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands;

/// <summary>
/// Conventional exit codes used by the CLI command base classes.
/// </summary>
[PublicAPI]
public static class ExitCodes
{
    /// <summary>The command completed successfully.</summary>
    public const int Success = 0;

    /// <summary>An unhandled error occurred during command execution.</summary>
    public const int Error = 1;

    /// <summary>The requested resource was not found (e.g. a Get command target).</summary>
    public const int NotFound = 2;

    /// <summary>The command was cancelled by the user (Ctrl+C) or by declining a confirmation prompt.</summary>
    public const int Cancelled = 130;
}

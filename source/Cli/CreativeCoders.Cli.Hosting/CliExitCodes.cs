namespace CreativeCoders.Cli.Hosting;

/// <summary>
/// Defines well-known exit codes used by the CLI hosting infrastructure.
/// </summary>
public static class CliExitCodes
{
    /// <summary>
    /// The exit code indicating successful execution.
    /// </summary>
    public const int Success = 0;

    /// <summary>
    /// The exit code indicating that no matching command was found for the given arguments.
    /// </summary>
    public const int CommandNotFound = int.MinValue;

    /// <summary>
    /// The exit code indicating that command creation failed.
    /// </summary>
    public const int CommandCreationFailed = int.MinValue + 1;

    /// <summary>
    /// The exit code indicating that the command result could not be determined.
    /// </summary>
    public const int CommandResultUnknown = int.MinValue + 2;

    /// <summary>
    /// The exit code indicating that the command options failed validation.
    /// </summary>
    public const int CommandOptionsInvalid = int.MinValue + 3;

    /// <summary>
    /// The exit code indicating that a pre-processor failed during execution.
    /// </summary>
    public const int PreProcessorFailed = int.MinValue + 4;

    /// <summary>
    /// The exit code indicating that a post-processor failed during execution.
    /// </summary>
    public const int PostProcessorFailed = int.MinValue + 5;
}

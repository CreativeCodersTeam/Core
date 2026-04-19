namespace CreativeCoders.Cli.Core;

/// <summary>
/// Specifies when a CLI pre-processor or post-processor should be executed.
/// </summary>
public enum CliProcessorExecutionCondition
{
    /// <summary>
    /// The processor is executed for every CLI invocation.
    /// </summary>
    Always,

    /// <summary>
    /// The processor is executed only when help output is displayed.
    /// </summary>
    OnlyOnHelp,

    /// <summary>
    /// The processor is executed only when a CLI command is run.
    /// </summary>
    OnlyOnCommand
}

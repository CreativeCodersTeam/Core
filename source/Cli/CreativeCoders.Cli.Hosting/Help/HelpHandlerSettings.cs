namespace CreativeCoders.Cli.Hosting.Help;

/// <summary>
/// Holds the configuration settings for the help handler.
/// </summary>
public class HelpHandlerSettings
{
    /// <summary>
    /// Gets the help command kinds that trigger help output.
    /// </summary>
    /// <value>An array of <see cref="HelpCommandKind"/> values. The default is an empty array.</value>
    public HelpCommandKind[] CommandKinds { get; init; } = [];
}

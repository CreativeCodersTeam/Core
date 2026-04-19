namespace CreativeCoders.Cli.Hosting.Help;

/// <summary>
/// Defines the types of help command invocation supported by the CLI application.
/// </summary>
public enum HelpCommandKind
{
    /// <summary>
    /// Help is invoked using the <c>help</c> command as the first argument.
    /// </summary>
    Command,

    /// <summary>
    /// Help is invoked using the <c>--help</c> argument.
    /// </summary>
    Argument,

    /// <summary>
    /// Help is displayed when no arguments are provided.
    /// </summary>
    EmptyArgs,

    /// <summary>
    /// Help is invoked using either the <c>help</c> command or the <c>--help</c> argument.
    /// </summary>
    CommandOrArgument
}

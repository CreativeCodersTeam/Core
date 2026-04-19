using CreativeCoders.Cli.Hosting.Commands.Store;

namespace CreativeCoders.Cli.Hosting.Help;

/// <summary>
/// Defines a handler for printing CLI command help information.
/// </summary>
public interface ICliCommandHelpHandler
{
    /// <summary>
    /// Determines whether help should be printed based on the provided arguments.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <returns><see langword="true"/> if help should be printed; otherwise, <see langword="false"/>.</returns>
    bool ShouldPrintHelp(string[] args);

    /// <summary>
    /// Prints help output based on the provided arguments.
    /// </summary>
    /// <param name="args">The command line arguments used to determine which help to display.</param>
    void PrintHelp(string[] args);

    /// <summary>
    /// Prints help output for the specified collection of tree nodes.
    /// </summary>
    /// <param name="nodeChildNodes">The list of CLI tree nodes to display help for.</param>
    void PrintHelpFor(IList<CliTreeNode> nodeChildNodes);
}

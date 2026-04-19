using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Cli.Hosting.Commands.Store;

namespace CreativeCoders.Cli.Hosting.Help;

/// <summary>
/// Provides a no-op implementation of <see cref="ICliCommandHelpHandler"/> used when help is disabled.
/// </summary>
[ExcludeFromCodeCoverage]
public class DisabledCommandHelpHandler : ICliCommandHelpHandler
{
    /// <inheritdoc />
    public bool ShouldPrintHelp(string[] args) => false;

    /// <inheritdoc />
    public void PrintHelp(string[] args) { }

    /// <inheritdoc />
    public void PrintHelpFor(IList<CliTreeNode> nodeChildNodes) { }
}

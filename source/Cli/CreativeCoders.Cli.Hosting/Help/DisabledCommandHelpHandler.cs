using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Cli.Hosting.Commands.Store;

namespace CreativeCoders.Cli.Hosting.Help;

[ExcludeFromCodeCoverage]
public class DisabledCommandHelpHandler : ICliCommandHelpHandler
{
    public bool ShouldPrintHelp(string[] args) => false;

    public void PrintHelp(string[] args) { }

    public void PrintHelpFor(IList<CliTreeNode> nodeChildNodes) { }
}

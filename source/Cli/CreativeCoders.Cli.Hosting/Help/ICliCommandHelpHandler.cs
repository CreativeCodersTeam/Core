using CreativeCoders.Cli.Hosting.Commands.Store;

namespace CreativeCoders.Cli.Hosting.Help;

public interface ICliCommandHelpHandler
{
    bool ShouldPrintHelp(string[] args);

    void PrintHelp(string[] args);

    void PrintHelpFor(IList<CliTreeNode> nodeChildNodes);
}

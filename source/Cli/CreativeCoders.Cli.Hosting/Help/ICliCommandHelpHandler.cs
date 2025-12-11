namespace CreativeCoders.Cli.Hosting.Help;

public interface ICliCommandHelpHandler
{
    bool ShouldPrintHelp(string[] args);

    void PrintHelp(string[] args);
}

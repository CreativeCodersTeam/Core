namespace CreativeCoders.Cli.Hosting.Help;

public class DisabledCommandHelpHandler : ICliCommandHelpHandler
{
    public bool ShouldPrintHelp(string[] args)
        => false;

    public void PrintHelp(string[] args) { }
}

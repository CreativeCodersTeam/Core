using CreativeCoders.Core;

namespace CreativeCoders.Cli.Hosting.Help;

public class CliCommandHelpHandler(HelpHandlerSettings settings) : ICliCommandHelpHandler
{
    private readonly HelpHandlerSettings _settings = Ensure.NotNull(settings);

    public bool ShouldPrintHelp(string[] args)
    {
        var lowerCaseArgs = args.Select(x => x.ToLower()).ToArray();

        return _settings.CommandKind switch
        {
            HelpCommandKind.Command => lowerCaseArgs.FirstOrDefault() == "help",
            HelpCommandKind.Argument => lowerCaseArgs.Contains("--help"),
            HelpCommandKind.CommandOrArgument => lowerCaseArgs.FirstOrDefault() == "help" ||
                                                 lowerCaseArgs.Contains("--help"),
            _ => false
        };
    }

    public void PrintHelp(string[] args) { }
}

namespace CreativeCoders.Cli.Hosting.Commands;

public class CliCommandStore
{
    private readonly IEnumerable<CliCommandInfo> _commands;

    public CliCommandStore(IEnumerable<CliCommandInfo> commands)
    {
        _commands = commands;
    }

    public IEnumerable<CliCommandInfo> Commands => _commands;
}

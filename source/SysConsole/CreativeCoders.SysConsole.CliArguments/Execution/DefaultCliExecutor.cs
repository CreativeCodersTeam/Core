using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.Cli.Parsing;
using CreativeCoders.SysConsole.CliArguments.Commands;

namespace CreativeCoders.SysConsole.CliArguments.Execution;

public class DefaultCliExecutor : ICliExecutor
{
    private readonly ExecutionContext _context;

    public DefaultCliExecutor(ExecutionContext context)
    {
        _context = Ensure.NotNull(context, nameof(context));
    }

    public async Task<int> ExecuteAsync(string[] args)
    {
        Ensure.NotNull(args, nameof(args));

        var (groupCommandIsExecuted, groupCommandResult) = await TryExecuteGroupCommandAsync(args).ConfigureAwait(false);

        if (groupCommandIsExecuted)
        {
            return groupCommandResult?.ReturnCode ?? _context.DefaultErrorReturnCode;
        }

        var (commandIsExecuted, commandResult) = await TryExecuteCommandAsync(args).ConfigureAwait(false);

        if (commandIsExecuted)
        {
            return commandResult?.ReturnCode ?? _context.DefaultErrorReturnCode;
        }

        if (_context.DefaultCommand == null)
        {
            return _context.DefaultErrorReturnCode;
        }

        var defaultOptions = new OptionParser(_context.DefaultCommand.OptionsType).Parse(args);

        var defaultCommandResult = await _context.DefaultCommand.ExecuteAsync(defaultOptions).ConfigureAwait(false);

        return defaultCommandResult.ReturnCode;
    }

    private async Task<(bool IsExecuted, CliCommandResult? CommandResult)> TryExecuteCommandAsync(
        string[] args)
    {
        var command = _context.Commands.FirstOrDefault(x => x.Name == args.FirstOrDefault())
                      ?? _context.Commands.FirstOrDefault(x => x.IsDefault);

        if (command == null)
        {
            return (false, null);
        }

        var options = new OptionParser(command.OptionsType).Parse(args.Skip(1).ToArray());

        var commandResult = await command.ExecuteAsync(options).ConfigureAwait(false);

        return (true, commandResult);
    }

    private async Task<(bool IsExecuted, CliCommandResult? CommandResult)> TryExecuteGroupCommandAsync(
        string[] args)
    {
        var group = _context.CommandGroups.FirstOrDefault(x => x.Name == args.FirstOrDefault());

        var groupCommand = group?.Commands.FirstOrDefault(x => x.Name == args.Skip(1).FirstOrDefault())
                           ?? group?.Commands.FirstOrDefault(x => x.IsDefault);

        if (groupCommand == null)
        {
            return (false, null);
        }

        var options = new OptionParser(groupCommand.OptionsType).Parse(args.Skip(2).ToArray());

        return (true, await groupCommand.ExecuteAsync(options).ConfigureAwait(false));
    }

    public int DefaultErrorReturnCode => _context.DefaultErrorReturnCode;
}

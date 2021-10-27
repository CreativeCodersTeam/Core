using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.CliArguments.Commands;
using CreativeCoders.SysConsole.CliArguments.Parsing;

namespace CreativeCoders.SysConsole.CliArguments.Execution
{
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

            var (groupCommandIsExecuted, groupCommandResult) = await TryExecuteGroupCommandAsync(args);

            if (groupCommandIsExecuted)
            {
                return groupCommandResult?.ReturnCode ?? _context.DefaultErrorReturnCode;
            }

            var (commandIsExecuted, commandResult) = await TryExecuteCommandAsync(args);

            if (commandIsExecuted)
            {
                return commandResult?.ReturnCode ?? _context.DefaultErrorReturnCode;
            }

            if (_context.DefaultCommand == null)
            {
                return _context.DefaultErrorReturnCode;
            }

            var defaultOptions = new OptionParser().Parse(_context.DefaultCommand.OptionsType, args);

            var defaultCommandResult = await _context.DefaultCommand.ExecuteAsync(defaultOptions);

            return defaultCommandResult.ReturnCode;
        }

        private async Task<(bool IsExecuted, CliCommandResult? CommandResult)> TryExecuteCommandAsync(string[] args)
        {
            var command = _context.Commands.FirstOrDefault(x => x.Name == args.First());

            if (command == null)
            {
                return (false, null);
            }

            var options = new OptionParser().Parse(command.OptionsType, args.Skip(1).ToArray());

            var commandResult = await command.ExecuteAsync(options);

            return (true, commandResult);
        }

        private async Task<(bool IsExecuted, CliCommandResult? CommandResult)> TryExecuteGroupCommandAsync(string[] args)
        {
            var group = _context.CommandGroups.FirstOrDefault(x => x.Name == args.First());

            var groupCommand = group?.Commands.FirstOrDefault(x => x.Name == args.Skip(1).First());

            if (groupCommand == null)
            {
                return (false, null);
            }

            var options = new OptionParser().Parse(groupCommand.OptionsType, args.Skip(2).ToArray());

            return (true, await groupCommand.ExecuteAsync(options));
        }
    }
}

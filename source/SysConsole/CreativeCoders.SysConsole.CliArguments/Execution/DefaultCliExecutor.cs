using System;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.CliArguments.Commands;

namespace CreativeCoders.SysConsole.CliArguments.Execution
{
    public class DefaultCliExecutor : ICliExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ExecutionContext _context;

        public DefaultCliExecutor(ExecutionContext context, IServiceProvider serviceProvider)
        {
            _context = Ensure.NotNull(context, nameof(context));
            _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));
        }

        public async Task<int> ExecuteAsync(string[] args)
        {
            Ensure.NotNull(args, nameof(args));

            var (groupCommandIsExecuted, groupCommandResult) = await TryExecuteGroupCommandAsync(args);

            if (groupCommandIsExecuted)
            {
                return groupCommandResult?.ReturnCode ?? int.MinValue;
            }

            var (commandIsExecuted, commandResult) = await TryExecuteCommandAsync(args);

            if (commandIsExecuted)
            {
                return commandResult?.ReturnCode ?? int.MinValue;
            }

            if (_context.DefaultCommand == null)
            {
                return int.MinValue;
            }

            var defaultOptions = _context.DefaultCommand.OptionsType.CreateInstance<object>(_serviceProvider);

            if (defaultOptions == null)
            {
                throw new InvalidOperationException();
            }

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

            var options = command.OptionsType.CreateInstance<object>(_serviceProvider);

            if (options == null)
            {
                throw new InvalidOperationException();
            }

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

            var options = groupCommand.OptionsType.CreateInstance<object>(_serviceProvider);

            if (options == null)
            {
                throw new InvalidOperationException();
            }

            return (true, await groupCommand.ExecuteAsync(options));
        }
    }
}

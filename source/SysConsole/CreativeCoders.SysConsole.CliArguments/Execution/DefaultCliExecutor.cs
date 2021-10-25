using System;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core.Reflection;

namespace CreativeCoders.SysConsole.CliArguments.Execution
{
    public class DefaultCliExecutor : ICliExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ExecutionContext _context;

        public DefaultCliExecutor(ExecutionContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public async Task<int> ExecuteAsync(string[] args)
        {
            var group = _context.CommandGroups.FirstOrDefault(x => x.Name == args.First());

            var groupCommand = group?.Commands.FirstOrDefault(x => x.Name == args.Skip(1).First());

            if (groupCommand != null)
            {
                var options = groupCommand.OptionsType.CreateInstance<object>(_serviceProvider);

                if (options == null)
                {
                    throw new InvalidOperationException();
                }

                var groupCommandResult = await groupCommand.ExecuteAsync(options);

                return groupCommandResult.ReturnCode;
            }

            var command = _context.Commands.FirstOrDefault(x => x.Name == args.First());

            if (command != null)
            {
                var options = command.OptionsType.CreateInstance<object>(_serviceProvider);

                if (options == null)
                {
                    throw new InvalidOperationException();
                }

                var commandResult = await command.ExecuteAsync(options);

                return commandResult.ReturnCode;
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
    }
}

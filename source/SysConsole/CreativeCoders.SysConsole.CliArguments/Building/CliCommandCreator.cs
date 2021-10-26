using System;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.CliArguments.Commands;
using CreativeCoders.SysConsole.CliArguments.Exceptions;

namespace CreativeCoders.SysConsole.CliArguments.Building
{
    public class CliCommandCreator
    {
        private readonly IServiceProvider _serviceProvider;

        public CliCommandCreator(IServiceProvider serviceProvider)
        {
            _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));
        }

        public ICliCommand CreateCommand<TCommand>()
            where TCommand : class, ICliCommand
        {
            var command = typeof(TCommand).CreateInstance<ICliCommand>(_serviceProvider);

            if (command == null)
            {
                throw new CliCommandCreationFailedException(typeof(TCommand));
            }

            return command;
        }

        public ICliCommand CreateCommand<TCommand, TOptions>(Action<TCommand> configureCommand)
            where TCommand : class, ICliCommand<TOptions>
            where TOptions : class, new()
        {
            Ensure.NotNull(configureCommand, nameof(configureCommand));

            var command = typeof(TCommand).CreateInstance<TCommand>(_serviceProvider);

            if (command == null)
            {
                throw new CliCommandCreationFailedException(typeof(TCommand));
            }

            configureCommand(command);

            return command;
        }
    }
}

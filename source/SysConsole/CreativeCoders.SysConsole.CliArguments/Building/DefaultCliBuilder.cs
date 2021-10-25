using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.CliArguments.Commands;
using CreativeCoders.SysConsole.CliArguments.Exceptions;
using CreativeCoders.SysConsole.CliArguments.Execution;

namespace CreativeCoders.SysConsole.CliArguments.Building
{
    public class DefaultCliBuilder : ICliBuilder
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IList<Func<ICliCommand>> _commandCreators;

        private readonly IList<Func<ICliCommandGroup>> _commandGroupCreators;

        private Func<ICliCommand>? _defaultCommandCreator;

        public DefaultCliBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));

            _commandCreators = new List<Func<ICliCommand>>();
            _commandGroupCreators = new List<Func<ICliCommandGroup>>();
        }

        public ICliBuilder AddCommand<TCommand, TOptions>(Action<TCommand> configureCommand)
            where TCommand : class, ICliCommand<TOptions>
            where TOptions : class, new()
        {

            _commandCreators.Add(() => CreateCommand<TCommand, TOptions>(configureCommand));

            return this;
        }

        public ICliBuilder AddCommandGroup(ICliCommandGroup commandGroup)
        {
            _commandGroupCreators.Add(() => commandGroup);

            return this;
        }

        public ICliBuilder AddDefaultCommand<TCommand, TOptions>(Action<TCommand> configureCommand) where TCommand : class, ICliCommand<TOptions> where TOptions : class, new()
        {
            _defaultCommandCreator = () => CreateCommand<TCommand, TOptions>(configureCommand);

            return this;
        }

        private ICliCommand CreateCommand<TCommand, TOptions>(Action<TCommand> configureCommand)
            where TCommand : class, ICliCommand<TOptions>
            where TOptions : class, new()
        {
            var command = typeof(TCommand).CreateInstance<TCommand>(_serviceProvider);

            if (command == null)
            {
                throw new CliArgumentsException($"Command '{typeof(TCommand)}' can not be created");
            }

            configureCommand(command);

            return command;
        }

        public ICliExecutor BuildExecutor()
        {
            var commands = _commandCreators.Select(x => x());

            var commandGroups = _commandGroupCreators.Select(x => x());

            var context = new ExecutionContext(commandGroups, commands, _defaultCommandCreator?.Invoke());

            return new DefaultCliExecutor(context, _serviceProvider);
        }
    }
}

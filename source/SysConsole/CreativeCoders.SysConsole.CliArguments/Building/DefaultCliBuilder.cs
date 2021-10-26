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

        private readonly CliCommandCreator _commandCreator;

        public DefaultCliBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));

            _commandCreators = new List<Func<ICliCommand>>();
            _commandGroupCreators = new List<Func<ICliCommandGroup>>();

            _commandCreator = new CliCommandCreator(serviceProvider);
        }

        public ICliBuilder AddCommand<TCommand, TOptions>(Action<TCommand> configureCommand)
            where TCommand : class, ICliCommand<TOptions>
            where TOptions : class, new()
        {
            _commandCreators.Add(() => _commandCreator.CreateCommand<TCommand, TOptions>(configureCommand));

            return this;
        }

        public ICliBuilder AddCommand<TCommand>()
            where TCommand : class, ICliCommand
        {
            _commandCreators.Add(_commandCreator.CreateCommand<TCommand>);

            return this;
        }

        public ICliBuilder AddCommandGroup(ICliCommandGroup commandGroup)
        {
            Ensure.NotNull(commandGroup, nameof(commandGroup));

            _commandGroupCreators.Add(() => commandGroup);

            return this;
        }

        public ICliBuilder AddCommandGroup(Action<ICliCommandGroupBuilder> configureGroupBuilder)
        {
            Ensure.NotNull(configureGroupBuilder, nameof(configureGroupBuilder));

            _commandGroupCreators.Add(() => CreateGroup(configureGroupBuilder));

            return this;
        }

        private ICliCommandGroup CreateGroup(Action<ICliCommandGroupBuilder> configureGroupBuilder)
        {
            var builder = new DefaultCliCommandGroupBuilder(_serviceProvider);

            configureGroupBuilder(builder);

            return builder.CreateGroup();
        }

        public ICliBuilder AddDefaultCommand<TCommand, TOptions>(Action<TCommand> configureCommand)
            where TCommand : class, ICliCommand<TOptions>
            where TOptions : class, new()
        {
            Ensure.NotNull(configureCommand, nameof(configureCommand));

            _defaultCommandCreator = () => _commandCreator.CreateCommand<TCommand, TOptions>(configureCommand);

            return this;
        }

        public ICliBuilder AddModule(ICliModule cliModule)
        {
            Ensure.NotNull(cliModule, nameof(cliModule));

            cliModule.Configure(this);

            return this;
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

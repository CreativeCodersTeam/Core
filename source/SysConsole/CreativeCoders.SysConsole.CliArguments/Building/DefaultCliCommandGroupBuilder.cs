using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.CliArguments.Commands;

namespace CreativeCoders.SysConsole.CliArguments.Building;

public class DefaultCliCommandGroupBuilder : ICliCommandGroupBuilder
{
    private readonly List<Func<ICliCommand>> _commandCreators;

    private readonly CliCommandFactory _commandFactory;

    private string? _name;

    public DefaultCliCommandGroupBuilder(IServiceProvider serviceProvider)
    {
        Ensure.NotNull(serviceProvider);

        _commandCreators = [];
        _commandFactory = new CliCommandFactory(serviceProvider);
    }

    public ICliCommandGroup CreateGroup()
    {
        return new CliCommandGroup
        {
            Name = Ensure.IsNotNullOrWhitespace(_name),
            Commands = _commandCreators.Select(x => x())
        };
    }

    public ICliCommandGroupBuilder SetName(string name)
    {
        _name = Ensure.IsNotNullOrWhitespace(name);

        return this;
    }

    public ICliCommandGroupBuilder AddCommand<TCommand, TOptions>(Action<TCommand> configureCommand)
        where TCommand : class, ICliCommand<TOptions>
        where TOptions : class, new()
    {
        Ensure.NotNull(configureCommand);

        _commandCreators.Add(() => _commandFactory.CreateCommand<TCommand, TOptions>(configureCommand));

        return this;
    }

    public ICliCommandGroupBuilder AddCommand<TCommand>()
        where TCommand : class, ICliCommand
    {
        _commandCreators.Add(() => _commandFactory.CreateCommand<TCommand>());

        return this;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.CliArguments.Commands;

namespace CreativeCoders.SysConsole.CliArguments.Building;

public class DefaultCliCommandGroupBuilder : ICliCommandGroupBuilder
{
    private string? _name;

    private readonly CliCommandFactory _commandFactory;

    private readonly IList<Func<ICliCommand>> _commandCreators;

    public DefaultCliCommandGroupBuilder(IServiceProvider serviceProvider)
    {
        Ensure.NotNull(serviceProvider, nameof(serviceProvider));

        _commandCreators = new List<Func<ICliCommand>>();
        _commandFactory = new CliCommandFactory(serviceProvider);
    }

    public ICliCommandGroupBuilder SetName(string name)
    {
        _name = Ensure.IsNotNullOrWhitespace(name, nameof(name));

        return this;
    }

    public ICliCommandGroupBuilder AddCommand<TCommand, TOptions>(Action<TCommand> configureCommand)
        where TCommand : class, ICliCommand<TOptions>
        where TOptions : class, new()
    {
        Ensure.NotNull(configureCommand, nameof(configureCommand));

        _commandCreators.Add(() => _commandFactory.CreateCommand<TCommand, TOptions>(configureCommand));

        return this;
    }

    public ICliCommandGroupBuilder AddCommand<TCommand>()
        where TCommand : class, ICliCommand
    {
        _commandCreators.Add(() => _commandFactory.CreateCommand<TCommand>());

        return this;
    }

    public ICliCommandGroup CreateGroup()
    {
        return new CliCommandGroup
        {
            Name = Ensure.IsNotNullOrWhitespace(_name, nameof(_name)),
            Commands = _commandCreators.Select(x => x())
        };
    }
}
using System;
using CreativeCoders.SysConsole.CliArguments.Commands;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.Building;

[PublicAPI]
public interface ICliCommandGroupBuilder
{
    ICliCommandGroupBuilder SetName(string name);

    ICliCommandGroupBuilder AddCommand<TCommand, TOptions>(Action<TCommand> configureCommand)
        where TCommand : class, ICliCommand<TOptions>
        where TOptions : class, new();

    ICliCommandGroupBuilder AddCommand<TCommand>()
        where TCommand : class, ICliCommand;
}
using System;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.CliArguments.Commands;
using CreativeCoders.SysConsole.CliArguments.Exceptions;

namespace CreativeCoders.SysConsole.CliArguments.Building;

public class CliCommandFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CliCommandFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));
    }

    public ICliCommand CreateCommand<TCommand>()
        where TCommand : class, ICliCommand
    {
        try
        {
            var command = typeof(TCommand).CreateInstance<ICliCommand>(_serviceProvider);

            return command!;
        }
        catch (Exception e)
        {
            throw new CliCommandCreationFailedException(typeof(TCommand), e);
        }
    }

    public ICliCommand CreateCommand<TCommand, TOptions>(Action<TCommand> configureCommand)
        where TCommand : class, ICliCommand<TOptions>
        where TOptions : class, new()
    {
        Ensure.NotNull(configureCommand, nameof(configureCommand));

        try
        {
            var command = typeof(TCommand).CreateInstance<TCommand>(_serviceProvider)!;

            configureCommand(command);

            return command;
        }
        catch (Exception e)
        {
            throw new CliCommandCreationFailedException(typeof(TCommand), e);
        }
    }
}
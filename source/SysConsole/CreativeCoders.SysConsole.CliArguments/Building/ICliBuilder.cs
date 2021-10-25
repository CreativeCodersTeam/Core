using System;
using CreativeCoders.SysConsole.CliArguments.Commands;
using CreativeCoders.SysConsole.CliArguments.Execution;

namespace CreativeCoders.SysConsole.CliArguments.Building
{
    public interface ICliBuilder
    {
        ICliBuilder AddCommand<TCommand, TOptions>(Action<TCommand> configureCommand)
            where TCommand : class, ICliCommand<TOptions>
            where TOptions : class, new();

        ICliBuilder AddCommand<TCommand>()
            where TCommand : class, ICliCommand;

        ICliBuilder AddCommandGroup(ICliCommandGroup commandGroup);

        ICliBuilder AddDefaultCommand<TCommand, TOptions>(Action<TCommand> configureCommand)
            where TCommand : class, ICliCommand<TOptions>
            where TOptions : class, new();

        ICliExecutor BuildExecutor();
    }

    public interface ICliCommandGroupBuilder
    {
        ICliCommandGroupBuilder SetName(string name);

        ICliCommandGroupBuilder AddCommand<TCommand, TOptions>(Action<TCommand> configureCommand)
            where TCommand : class, ICliCommand<TOptions>
            where TOptions : class, new();
    }
}

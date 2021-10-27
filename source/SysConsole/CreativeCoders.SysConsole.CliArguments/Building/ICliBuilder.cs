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

        ICliBuilder AddCommandGroup(Action<ICliCommandGroupBuilder> configureGroupBuilder);

        ICliBuilder AddDefaultCommand<TCommand, TOptions>(Action<TCommand> configureCommand)
            where TCommand : class, ICliCommand<TOptions>
            where TOptions : class, new();

        ICliBuilder SetDefaultErrorReturnCode(int errorReturnCode);

        ICliExecutor BuildExecutor();
    }
}

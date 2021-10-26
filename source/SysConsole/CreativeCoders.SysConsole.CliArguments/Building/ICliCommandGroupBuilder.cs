using System;
using CreativeCoders.SysConsole.CliArguments.Commands;

namespace CreativeCoders.SysConsole.CliArguments.Building
{
    public interface ICliCommandGroupBuilder
    {
        ICliCommandGroupBuilder SetName(string name);

        ICliCommandGroupBuilder AddCommand<TCommand, TOptions>(Action<TCommand> configureCommand)
            where TCommand : class, ICliCommand<TOptions>
            where TOptions : class, new();
    }
}

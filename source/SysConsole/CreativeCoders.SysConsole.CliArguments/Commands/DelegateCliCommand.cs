using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.CliArguments.Exceptions;

namespace CreativeCoders.SysConsole.CliArguments.Commands
{
    public class DelegateCliCommand<TOptions> : CliCommandBase<TOptions>
        where TOptions : class, new()
    {
        public override Task<CliCommandResult> ExecuteAsync(TOptions options)
        {
            return OnExecuteAsync?.Invoke(options)
                   ?? throw new CliArgumentsException("No command execution function defined");
        }

        public override string Name { get; set; } = string.Empty;

        public Func<TOptions, Task<CliCommandResult>> OnExecuteAsync { get; set; } =
            _ => throw new NotImplementedException();
    }
}

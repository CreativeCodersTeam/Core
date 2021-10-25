using System;
using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.CliArguments.Commands
{
    public abstract class CliCommandBase<TOptions> : ICliCommand<TOptions>
        where TOptions : class, new()
    {
        public abstract Task<CliCommandResult> ExecuteAsync(TOptions options);

        public Task<CliCommandResult> ExecuteAsync(object options)
        {
            if (options is not TOptions commandOptions)
            {
                throw new InvalidOperationException();
            }

            return ExecuteAsync(commandOptions);
        }

        public Type OptionsType => typeof(TOptions);

        public abstract string Name { get; set; }
    }
}

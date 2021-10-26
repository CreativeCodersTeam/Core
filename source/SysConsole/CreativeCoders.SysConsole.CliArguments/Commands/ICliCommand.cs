using System;
using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.CliArguments.Commands
{
    public interface ICliCommand
    {
        public string Name { get; set; }

        Task<CliCommandResult> ExecuteAsync(object options);

        Type OptionsType { get; }
    }

    public interface ICliCommand<in TOptions> : ICliCommand
        where TOptions : class, new()
    {
        Task<CliCommandResult> ExecuteAsync(TOptions options);
    }
}

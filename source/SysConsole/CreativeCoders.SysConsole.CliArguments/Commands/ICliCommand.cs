using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.Commands;

[PublicAPI]
public interface ICliCommand
{
    public string Name { get; set; }

    Task<CliCommandResult> ExecuteAsync(object options);

    Type OptionsType { get; }

    bool IsDefault { get; set; }
}

[PublicAPI]
public interface ICliCommand<in TOptions> : ICliCommand
    where TOptions : class, new()
{
    Task<CliCommandResult> ExecuteAsync(TOptions options);
}
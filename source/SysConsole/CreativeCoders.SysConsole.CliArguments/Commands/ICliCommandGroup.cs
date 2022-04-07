using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.Commands;

[PublicAPI]
public interface ICliCommandGroup
{
    public string Name { get; set; }

    IEnumerable<ICliCommand> Commands { get; }
}

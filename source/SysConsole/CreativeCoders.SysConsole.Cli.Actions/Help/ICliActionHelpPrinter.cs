using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.SysConsole.Cli.Actions.Help;

public interface ICliActionHelpPrinter
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    void PrintHelp(IEnumerable<string> actionRouteParts);
}
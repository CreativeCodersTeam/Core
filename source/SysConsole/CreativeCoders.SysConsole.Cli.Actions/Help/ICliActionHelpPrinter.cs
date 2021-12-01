using System.Collections.Generic;

namespace CreativeCoders.SysConsole.Cli.Actions.Help
{
    public interface ICliActionHelpPrinter
    {
        void PrintHelp(IEnumerable<string> actionRouteParts);
    }
}

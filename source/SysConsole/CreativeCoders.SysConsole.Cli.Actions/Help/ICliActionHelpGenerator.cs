using System.Collections.Generic;

namespace CreativeCoders.SysConsole.Cli.Actions.Help;

public interface ICliActionHelpGenerator
{
    CliActionHelp CreateHelp(IEnumerable<string> actionRouteParts);
}
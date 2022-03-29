using System;

namespace CreativeCoders.SysConsole.Cli.Parsing.Help;

public interface IOptionsHelpGenerator
{
    OptionsHelp CreateHelp(Type optionsType);
}
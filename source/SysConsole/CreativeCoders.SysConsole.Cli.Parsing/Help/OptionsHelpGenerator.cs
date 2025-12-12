using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;

namespace CreativeCoders.SysConsole.Cli.Parsing.Help;

public class OptionsHelpGenerator : IOptionsHelpGenerator
{
    public OptionsHelp CreateHelp(Type optionsType)
    {
        Ensure.NotNull(optionsType);

        return new OptionsHelp(
            CreateValueHelpEntries(optionsType),
            CreateParameterHelpEntries(optionsType));
    }

    private static IEnumerable<HelpEntry> CreateValueHelpEntries(Type optionsType)
    {
        var valueAttributes = GetOptionValues(optionsType);

        return valueAttributes.Select(x =>
            new HelpEntry
            {
                ArgumentName = string.IsNullOrEmpty(x.Option.Name)
                    ? x.OptionProperty.Name
                    : $"<{x.Option.Name.ToUpper()}>",
                HelpText = x.Option.HelpText
            });
    }

    private static IEnumerable<HelpEntry> CreateParameterHelpEntries(Type optionsType)
    {
        var parameterAttributes = GetOptionParameters(optionsType);

        return parameterAttributes.Select(x =>
            new HelpEntry
            {
                ArgumentName = GetArgumentName(x),
                HelpText = x.HelpText
            });
    }

    private static string GetArgumentName(OptionParameterAttribute optionParameter)
    {
        var argName = optionParameter.ShortName != null
            ? $"-{optionParameter.ShortName} "
            : string.Empty;

        if (!string.IsNullOrEmpty(optionParameter.LongName))
        {
            argName += $"--{optionParameter.LongName} ";
        }

        if (!string.IsNullOrEmpty(optionParameter.Name))
        {
            argName += $"<{optionParameter.Name.ToUpper()}>";
        }

        return argName.Trim();
    }

    private static IEnumerable<(OptionValueAttribute Option, PropertyInfo OptionProperty)> GetOptionValues(
        Type optionsType)
    {
        return
            from property in optionsType.GetProperties()
            let attribute =
                property.GetCustomAttribute<OptionValueAttribute>()
            where attribute != null
            select (attribute, property);
    }

    private static IEnumerable<OptionParameterAttribute> GetOptionParameters(Type optionsType)
    {
        return
            from property in optionsType.GetProperties()
            let attribute =
                property.GetCustomAttribute<OptionParameterAttribute>()
            where attribute != null
            select attribute;
    }
}

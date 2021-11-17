using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;

namespace CreativeCoders.SysConsole.Cli.Parsing.Help
{
    public class OptionsHelpGenerator : IOptionsHelpGenerator
    {
        private readonly Type _optionsType;

        public OptionsHelpGenerator(Type optionsType)
        {
            _optionsType = Ensure.NotNull(optionsType, nameof(optionsType));
        }

        public OptionsHelp CreateHelp()
        {
            return new OptionsHelp(CreateValueHelpEntries(), CreateParameterHelpEntries());
        }

        private IEnumerable<HelpEntry> CreateValueHelpEntries()
        {
            var valueAttributes = GetOptionValues();

            return valueAttributes.Select(x =>
                new HelpEntry
                {
                    ArgumentName = string.IsNullOrEmpty(x.Name) ? string.Empty : $"<{x.Name.ToUpper()}>",
                    HelpText = x.HelpText
                });
        }

        private IEnumerable<HelpEntry> CreateParameterHelpEntries()
        {
            var parameterAttributes = GetOptionParameters();

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

        private IEnumerable<OptionValueAttribute> GetOptionValues()
        {
            return
                from property in _optionsType.GetProperties()
                let attribute =
                    property.GetCustomAttribute(typeof(OptionValueAttribute)) as OptionValueAttribute
                where attribute != null
                select attribute;
        }

        private IEnumerable<OptionParameterAttribute> GetOptionParameters()
        {
            return
                from property in _optionsType.GetProperties()
                let attribute =
                    property.GetCustomAttribute(typeof(OptionParameterAttribute)) as OptionParameterAttribute
                where attribute != null
                select attribute;
        }
    }
}

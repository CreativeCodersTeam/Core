using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;
using CreativeCoders.Core.IO;
using CreativeCoders.Core.SysEnvironment;
using CreativeCoders.SysConsole.Cli.Actions.Definition;
using CreativeCoders.SysConsole.Cli.Actions.Routing;
using CreativeCoders.SysConsole.Cli.Parsing.Help;

namespace CreativeCoders.SysConsole.Cli.Actions.Help
{
    public class CliActionHelpGenerator : ICliActionHelpGenerator
    {
        private readonly ICliActionRouter _actionRouter;

        private readonly IOptionsHelpGenerator _optionsHelpGenerator;

        public CliActionHelpGenerator(IOptionsHelpGenerator optionsHelpGenerator,
            ICliActionRouter actionRouter)
        {
            _actionRouter = Ensure.NotNull(actionRouter, nameof(actionRouter));
            _optionsHelpGenerator = Ensure.NotNull(optionsHelpGenerator, nameof(optionsHelpGenerator));
        }

        public CliActionHelp CreateHelp(IEnumerable<string> actionRouteParts)
        {
            var actionParts = actionRouteParts.ToList();

            var route = _actionRouter.FindRoute(actionParts);

            if (route == null)
            {
                throw new InvalidOperationException();
            }

            var actionAttribute = route.ActionMethod.GetCustomAttribute<CliActionAttribute>();

            if (actionAttribute == null)
            {
                throw new InvalidOperationException();
            }

            var parameterInfo = route.ActionMethod.GetParameters().FirstOrDefault();

            var optionsHelp = parameterInfo != null
                ? _optionsHelpGenerator.CreateHelp(parameterInfo.ParameterType)
                : new OptionsHelp(Array.Empty<HelpEntry>(), Array.Empty<HelpEntry>());

            return new CliActionHelp(optionsHelp)
            {
                HelpText = actionAttribute.HelpText ?? route.ActionMethod.Name,
                Syntax = GetSyntax(actionParts, optionsHelp)
            };
        }

        private static string GetSyntax(IEnumerable<string> actionRouteParts, OptionsHelp optionsHelp)
        {
            var appName = FileSys.Path.GetFileNameWithoutExtension(Env.GetAppFileName());

            var action = string.Join(" ", actionRouteParts);

            var arguments = string.Join(" ", optionsHelp.ValueHelpEntries.Select(x => $"[{x.ArgumentName}]"));

            var syntaxParts = new List<string> {appName};

            if (!string.IsNullOrEmpty(action))
            {
                syntaxParts.Add(action);
            }
            
            if (!string.IsNullOrEmpty(arguments))
            {
                syntaxParts.Add(arguments);
            }
            
            if (optionsHelp.ParameterHelpEntries.Any())
            {
                syntaxParts.Add("[OPTIONS]");
            }

            return string.Join(" ", syntaxParts);
        }
    }
}

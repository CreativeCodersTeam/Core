using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;
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
            var route = _actionRouter.FindRoute(actionRouteParts.ToList());

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
                Syntax = GetSyntax()
            };
        }

        private static string GetSyntax()
        {
            return $"{Env.GetAppFileName()} [ARGUMENTS] [OPTIONS]";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using CreativeCoders.Core;

namespace CreativeCoders.SysConsole.App.Verbs
{
    internal class DefaultConsoleApplicationVerbBuilder : IConsoleApplicationVerbBuilder
    {
        private readonly IList<Type> _verbTypes;

        private Type? _errorHandlerType;

        public DefaultConsoleApplicationVerbBuilder()
        {
            _verbTypes = new List<Type>();
        }
        
        public IConsoleApplicationVerbBuilder AddVerb<TVerb>()
            where TVerb : class, IVerb
        {
            _verbTypes.Add(typeof(TVerb));

            return this;
        }

        public IConsoleApplicationVerbBuilder AddErrors<TErrorHandler>() where TErrorHandler : IVerbParserErrorHandler
        {
            _errorHandlerType = typeof(TErrorHandler);

            return this;
        }

        public IMain BuildMain(IMain? defaultMain, string[]? arguments,
            IServiceProvider serviceProvider)
        {
            Ensure.Argument(serviceProvider, nameof(serviceProvider)).NotNull();

            return new DelegateMain(() => Execute(defaultMain, arguments, serviceProvider));
        }

        private async Task<int> Execute(IMain? defaultMain, IEnumerable<string>? arguments, IServiceProvider serviceProvider)
        {
            var verbDefinitions = _verbTypes.Select(x => new VerbDefinition(x)).ToArray();

            return await Parser.Default
                .ParseArguments(arguments, verbDefinitions.Select(x => x.OptionsType).ToArray())
                .MapResult<object, Task<int>>(
                    async options => await ExecuteVerbAsync(options, verbDefinitions, serviceProvider),
                    async errors => await ExecuteErrorsAsync(errors, defaultMain, serviceProvider));
        }

        private async Task<int> ExecuteErrorsAsync(IEnumerable<Error> errors, IMain? defaultMain,
            IServiceProvider serviceProvider)
        {
            var errorHandler = _errorHandlerType?.CreateInstance<IVerbParserErrorHandler>(serviceProvider);

            if (errorHandler != null)
            {
                return await errorHandler.HandleErrorsAsync(errors).ConfigureAwait(false);
            }

            if (defaultMain == null)
            {
                return int.MinValue;
            }

            return await defaultMain.ExecuteAsync().ConfigureAwait(false);
        }

        private static async Task<int> ExecuteVerbAsync(object options, IEnumerable<VerbDefinition> verbDefinitions, IServiceProvider serviceProvider)
        {
            var verbDefinition = verbDefinitions.FirstOrDefault(x => x.OptionsType == options.GetType());

            var verb = verbDefinition?.CreateVerb(options, serviceProvider);

            return verb == null
                ? default
                : await verb.ExecuteAsync();
        }
    }
}

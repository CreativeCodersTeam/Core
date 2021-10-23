using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.App.Execution;

namespace CreativeCoders.SysConsole.App.Verbs
{
    public class VerbsExecutor : IExecutor
    {
        private readonly IEnumerable<VerbDefinition> _verbDefinitions;

        private readonly IVerbParserErrorHandler? _errorHandler;

        private readonly IServiceProvider _serviceProvider;

        public VerbsExecutor(IEnumerable<VerbDefinition> verbDefinitions, IVerbParserErrorHandler? errorHandler,
            IServiceProvider serviceProvider)
        {
            _verbDefinitions = Ensure.NotNull(verbDefinitions, nameof(verbDefinitions));
            _errorHandler = errorHandler;
            _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));
        }

        public async Task<ExecutorResult> TryExecuteAsync(string[] args)
        {
            var result = await Parser.Default
                .ParseArguments(args, _verbDefinitions.Select(x => x.OptionsType).ToArray())
                .MapResult<object, Task<int?>>(
                    async options => await ExecuteVerbAsync(options, _verbDefinitions, _serviceProvider),
                    async errors => await ExecuteErrorsAsync(errors));

            return new ExecutorResult(result != null, result ?? int.MinValue);
        }

        private async Task<int?> ExecuteErrorsAsync(IEnumerable<Error> errors)
        {
            if (_errorHandler != null)
            {
                return await _errorHandler.HandleErrorsAsync(errors).ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<int?> ExecuteVerbAsync(object options, IEnumerable<VerbDefinition> verbDefinitions, IServiceProvider serviceProvider)
        {
            var verbDefinition = verbDefinitions.FirstOrDefault(x => x.OptionsType == options.GetType());

            var verb = verbDefinition?.CreateVerb(options, serviceProvider);

            return verb == null
                ? default
                : await verb.ExecuteAsync();
        }
    }
}

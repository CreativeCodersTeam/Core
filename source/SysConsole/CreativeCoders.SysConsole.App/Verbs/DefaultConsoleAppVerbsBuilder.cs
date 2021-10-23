using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.App.Execution;

namespace CreativeCoders.SysConsole.App.Verbs
{
    internal class DefaultConsoleAppVerbsBuilder : IConsoleAppVerbsBuilder
    {
        private readonly IList<Type> _verbTypes;

        private Type? _errorHandlerType;

        public DefaultConsoleAppVerbsBuilder()
        {
            _verbTypes = new List<Type>();
        }
        
        public IConsoleAppVerbsBuilder AddVerb<TVerb>()
            where TVerb : class, IVerb
        {
            _verbTypes.Add(typeof(TVerb));

            return this;
        }

        public IConsoleAppVerbsBuilder AddErrors<TErrorHandler>() where TErrorHandler : IVerbParserErrorHandler
        {
            _errorHandlerType = typeof(TErrorHandler);

            return this;
        }

        public IExecutor CreateExecutor(IServiceProvider serviceProvider)
        {
            var verbDefinitions = _verbTypes.Select(x => new VerbDefinition(x)).ToArray();

            var errorHandler = _errorHandlerType?.CreateInstance<IVerbParserErrorHandler>(serviceProvider);

            return new VerbsExecutor(verbDefinitions, errorHandler, serviceProvider);
        }
    }
}

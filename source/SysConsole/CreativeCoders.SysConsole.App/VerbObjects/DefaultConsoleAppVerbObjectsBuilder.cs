using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.App.Verbs;

namespace CreativeCoders.SysConsole.App.VerbObjects
{
    internal class DefaultConsoleAppVerbObjectsBuilder : IConsoleAppVerbObjectsBuilder
    {
        private readonly List<VerbObjectDefinition> _verbObjectDefinitions;

        public DefaultConsoleAppVerbObjectsBuilder()
        {
            _verbObjectDefinitions = new List<VerbObjectDefinition>();
        }

        public IConsoleAppVerbObjectsBuilder AddObjects<TVerbObject>(Action<IConsoleAppVerbBuilder> verbBuilder)
            where TVerbObject : IVerbObject
        {
            _verbObjectDefinitions.Add(new VerbObjectDefinition(typeof(TVerbObject), verbBuilder));

            return this;
        }

        public async Task<ExecutionResult> TryExecute(IServiceProvider serviceProvider, string[] arguments)
        {
            if (arguments.Length <= 0)
            {
                return new ExecutionResult(false, 0);
            }

            var objectName = arguments[0];

            var executor = _verbObjectDefinitions
                .Select(x => x.CreateExecutor(serviceProvider, arguments))
                .FirstOrDefault(x => x.Name.Equals(objectName, StringComparison.CurrentCultureIgnoreCase));

            return executor != null
                ? new ExecutionResult(true, await executor.ExecuteAsync())
                : new ExecutionResult(false, 0);
        }
    }
}

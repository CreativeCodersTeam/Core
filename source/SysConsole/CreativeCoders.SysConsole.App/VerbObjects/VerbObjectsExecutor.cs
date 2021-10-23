using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.App.Execution;

namespace CreativeCoders.SysConsole.App.VerbObjects
{
    internal class VerbObjectsExecutor : IExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IEnumerable<VerbObjectDefinition> _verbObjectDefinitions;

        public VerbObjectsExecutor(IServiceProvider serviceProvider, IEnumerable<VerbObjectDefinition> verbObjectDefinitions)
        {
            _serviceProvider = serviceProvider;
            _verbObjectDefinitions = verbObjectDefinitions;
        }

        public async Task<ExecutorResult> TryExecuteAsync(string[] args)
        {
            if (args.Length <= 0)
            {
                return new ExecutorResult(false, 0);
            }

            var objectName = args[0];

            var executor = _verbObjectDefinitions
                .Select(x => x.CreateExecutor(_serviceProvider, args))
                .FirstOrDefault(x => x.Name.Equals(objectName, StringComparison.CurrentCultureIgnoreCase));

            return executor != null
                ? new ExecutorResult(true, await executor.ExecuteAsync())
                : new ExecutorResult(false, 0);
        }
    }
}

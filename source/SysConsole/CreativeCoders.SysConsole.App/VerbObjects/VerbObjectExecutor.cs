using System;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.App.Verbs;

namespace CreativeCoders.SysConsole.App.VerbObjects
{
    internal class VerbObjectExecutor
    {
        private readonly Action<IConsoleAppVerbsBuilder> _setupVerbBuilder;

        private readonly string[] _arguments;

        private readonly IServiceProvider _serviceProvider;

        public VerbObjectExecutor(IVerbObject? verbObject, Action<IConsoleAppVerbsBuilder> setupVerbBuilder,
            string[] arguments, IServiceProvider serviceProvider)
        {
            _setupVerbBuilder = setupVerbBuilder;
            _arguments = arguments;
            _serviceProvider = serviceProvider;
            Name = verbObject?.Name ?? string.Empty;
        }

        public async Task<int> ExecuteAsync()
        {
            var verbBuilder = new DefaultConsoleAppVerbsBuilder();

            _setupVerbBuilder(verbBuilder);

            var executor = verbBuilder.CreateExecutor(_serviceProvider);

            var executorResult = await executor.TryExecuteAsync(_arguments.Skip(1).ToArray()).ConfigureAwait(false);

            return executorResult.ReturnCode;
        }

        public string Name { get; }
    }
}

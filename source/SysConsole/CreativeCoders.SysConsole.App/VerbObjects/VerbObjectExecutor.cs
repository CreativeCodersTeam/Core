using System;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.App.Verbs;

namespace CreativeCoders.SysConsole.App.VerbObjects
{
    internal class VerbObjectExecutor
    {
        private readonly Action<IConsoleAppVerbBuilder> _verbBuilder;

        private readonly string[] _arguments;

        private readonly IServiceProvider _serviceProvider;

        public VerbObjectExecutor(IVerbObject? verbObject, Action<IConsoleAppVerbBuilder> verbBuilder,
            string[] arguments, IServiceProvider serviceProvider)
        {
            _verbBuilder = verbBuilder;
            _arguments = arguments;
            _serviceProvider = serviceProvider;
            Name = verbObject?.Name ?? string.Empty;
        }

        public async Task<int> ExecuteAsync()
        {
            var verbBuilder = new DefaultConsoleAppVerbBuilder();

            _verbBuilder(verbBuilder);

            return await verbBuilder
                .BuildMain(
                    new DelegateMain(() => Task.FromResult(-1)), _arguments.Skip(1).ToArray(), _serviceProvider)
                .ExecuteAsync();
        }

        public string Name { get; }
    }
}
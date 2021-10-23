using System;
using System.Collections.Generic;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.App.MainProgram;
using CreativeCoders.SysConsole.App.VerbObjects;
using CreativeCoders.SysConsole.App.Verbs;

namespace CreativeCoders.SysConsole.App.Execution
{
    public class ExecutorChain : IExecutorChain
    {
        private readonly Type? _programMainType;

        private readonly Action<IConsoleAppVerbsBuilder>? _setupVerbsBuilder;

        private readonly Action<IConsoleAppVerbObjectsBuilder>? _setupVerbObjectsBuilder;

        private readonly IServiceProvider _serviceProvider;

        public ExecutorChain(Type? programMainType, Action<IConsoleAppVerbsBuilder>? setupVerbsBuilder,
            Action<IConsoleAppVerbObjectsBuilder>? setupVerbObjectsBuilder, IServiceProvider serviceProvider)
        {
            _programMainType = programMainType;
            _setupVerbsBuilder = setupVerbsBuilder;
            _setupVerbObjectsBuilder = setupVerbObjectsBuilder;
            _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));
        }

        private IExecutor? CreateProgramMainExecutor()
        {
            var main = _programMainType?.CreateInstance<IMain>(_serviceProvider);

            if (_programMainType != null && main == null)
            {
                throw new ArgumentException("Program main can not be constructed");
            }

            return main != null
                ? new MainExecutor(main)
                : null;
        }

        private IExecutor? CreateVerbObjectsExecutor()
        {
            if (_setupVerbObjectsBuilder == null)
            {
                return null;
            }

            var verbObjectsBuilder = new DefaultConsoleAppVerbObjectsBuilder();

            _setupVerbObjectsBuilder(verbObjectsBuilder);

            return verbObjectsBuilder.CreateExecutor(_serviceProvider);
        }

        private IExecutor? CreateVerbsExecutor()
        {
            if (_setupVerbsBuilder == null)
            {
                return null;
            }

            var verbsBuilder = new DefaultConsoleAppVerbsBuilder();

            _setupVerbsBuilder(verbsBuilder);

            return verbsBuilder.CreateExecutor(_serviceProvider);
        }

        public IEnumerable<IExecutor> GetExecutors()
        {
            var programMainExecutor = CreateProgramMainExecutor();

            var verbObjectsExecutor = CreateVerbObjectsExecutor();

            if (verbObjectsExecutor != null)
            {
                yield return verbObjectsExecutor;
            }

            var verbsExecutor = CreateVerbsExecutor();

            if (verbsExecutor != null)
            {
                yield return verbsExecutor;
            }

            if (programMainExecutor != null)
            {
                yield return programMainExecutor;
            }
        }
    }
}

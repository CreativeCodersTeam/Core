using System;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.App.Verbs;

namespace CreativeCoders.SysConsole.App.VerbObjects
{
    internal class VerbObjectDefinition
    {
        private readonly Type _verbObjectType;

        private readonly Action<IConsoleAppVerbBuilder> _verbBuilder;

        private IVerbObject? _verbObject;

        public VerbObjectDefinition(Type verbObjectType, Action<IConsoleAppVerbBuilder> verbBuilder)
        {
            _verbObjectType = verbObjectType;
            _verbBuilder = verbBuilder;
        }

        public VerbObjectExecutor CreateExecutor(IServiceProvider serviceProvider, string[] arguments)
        {
            return new VerbObjectExecutor(CreateVerbObject(serviceProvider), _verbBuilder, arguments, serviceProvider);
        }

        private IVerbObject? CreateVerbObject(IServiceProvider serviceProvider)
        {
            return _verbObject ??= _verbObjectType.CreateInstance<IVerbObject>(serviceProvider);
        }
    }
}
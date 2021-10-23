using System;
using System.Collections.Generic;
using CreativeCoders.SysConsole.App.Execution;
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

        public IConsoleAppVerbObjectsBuilder AddObjects<TVerbObject>(Action<IConsoleAppVerbsBuilder> verbBuilder)
            where TVerbObject : IVerbObject
        {
            _verbObjectDefinitions.Add(new VerbObjectDefinition(typeof(TVerbObject), verbBuilder));

            return this;
        }

        public IExecutor CreateExecutor(IServiceProvider serviceProvider)
        {
            return new VerbObjectsExecutor(serviceProvider, _verbObjectDefinitions);
        }
    }
}

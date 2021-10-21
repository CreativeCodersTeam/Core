using System;
using CreativeCoders.SysConsole.App.Verbs;

namespace CreativeCoders.SysConsole.App.VerbObjects
{
    public interface IConsoleAppVerbObjectsBuilder
    {
        IConsoleAppVerbObjectsBuilder AddObjects<TVerbObject>(Action<IConsoleAppVerbBuilder> verbBuilder)
            where TVerbObject : IVerbObject;
    }
}

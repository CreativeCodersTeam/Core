using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.Base
{
    [PublicAPI]
    public interface IScript
    {
        T CreateObject<T>(IScriptContext scriptContext)
            where T : class;
        
        T CreateObject<T>()
            where T : class;
        
        IReadOnlyCollection<string> MethodNames { get; }
    }
}
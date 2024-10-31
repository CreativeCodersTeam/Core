using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.Base;

[PublicAPI]
public class ScriptContext : IScriptContext
{
    private readonly List<IInjection> _injections = new List<IInjection>();

    public void AddInjection(IInjection injection)
    {
        _injections.Add(injection);
    }

    public void SetupScriptObject(object scriptObject)
    {
        _injections.ForEach(x => x.Inject(scriptObject));
    }

    public static IScriptContext Empty { get; } = new ScriptContext();
}

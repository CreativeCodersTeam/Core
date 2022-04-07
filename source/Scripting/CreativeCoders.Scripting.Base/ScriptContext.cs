using System.Collections.Generic;
using CreativeCoders.Core.Collections;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.Base;

[PublicAPI]
public class ScriptContext : IScriptContext
{
    private readonly IList<IInjection> _injections;

    public ScriptContext()
    {
        _injections = new List<IInjection>();
    }

    public static IScriptContext Empty { get; } = new ScriptContext();

    public void AddInjection(IInjection injection)
    {
        _injections.Add(injection);
    }

    public void SetupScriptObject(object scriptObject)
    {
        _injections.ForEach(x => x.Inject(scriptObject));
    }
}

using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.CSharp.ClassTemplating;

[PublicAPI]
public class ScriptClassTemplate
{
    public ScriptClassTemplate()
    {
        Usings = new List<string>();
        Members = new ScriptClassMembers();
        ImplementsInterfaces = new List<string>();
        Injections = new ScriptClassInjections(this);
    }

    public IList<string> Usings { get; }

    public IList<string> ImplementsInterfaces { get; }

    public ScriptClassMembers Members { get; }

    public ScriptClassInjections Injections { get; }
}
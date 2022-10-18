using System.Collections.Generic;

namespace CreativeCoders.Scripting.CSharp;

public class CSharpScriptClassDefinition
{
    public CSharpScriptClassDefinition()
    {
        Usings = new List<string>();
    }

    public string NameSpace { get; init; }

    public string ClassName { get; init; }

    public string SourceCode { get; set; }

    public IList<string> Usings { get; }
}

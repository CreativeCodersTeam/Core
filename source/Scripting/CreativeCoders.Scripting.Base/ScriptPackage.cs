using JetBrains.Annotations;

namespace CreativeCoders.Scripting.Base;

[PublicAPI]
public class ScriptPackage
{
    public ScriptPackage(string id, string name, ISourceCode sourceCode)
    {
        Id = id;
        Name = name;
        SourceCode = sourceCode;
    }

    public string Id { get; }

    public string Name { get; }

    public ISourceCode SourceCode { get; }
}

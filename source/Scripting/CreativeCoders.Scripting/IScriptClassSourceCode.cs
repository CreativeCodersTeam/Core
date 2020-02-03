using JetBrains.Annotations;

namespace CreativeCoders.Scripting
{
    [PublicAPI]
    public interface IScriptClassSourceCode
    {
        string ClassName { get; }

        string NameSpace { get; }

        IScript Script { get; }

        string SourceCode { get; }
    }
}
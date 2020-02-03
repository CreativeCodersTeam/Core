using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting
{
    [PublicAPI]
    public interface IScript
    {
        string Id { get; }

        string ScriptLanguage { get; }

        string Name { get; }

        string SourceCode { get; }

        IEnumerable<string> Usings { get; }
    }
}

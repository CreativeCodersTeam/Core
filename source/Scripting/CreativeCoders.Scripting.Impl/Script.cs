using System.Collections.Generic;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.Impl
{
    [PublicAPI]
    public class Script : IScript
    {
        public const string CodePlaceHolder = "$$code$$";

        public Script(string name, string id, string scriptLanguage, string sourceCode, IEnumerable<string> usings)
        {
            Ensure.IsNotNullOrWhitespace(name, nameof(name));
            Ensure.IsNotNullOrWhitespace(id, nameof(id));
            Ensure.IsNotNullOrWhitespace(scriptLanguage, nameof(scriptLanguage));
            Ensure.IsNotNull(sourceCode, nameof(sourceCode));

            Name = name;
            Id = id;
            ScriptLanguage = scriptLanguage;
            SourceCode = sourceCode;
            Usings = usings;
        }

        public string Id { get; }

        public string ScriptLanguage { get; }

        public string Name { get; }

        public string SourceCode { get; }

        public IEnumerable<string> Usings { get; }
    }
}

using System.Collections.Generic;
using CreativeCoders.CodeCompilation;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.Exceptions
{
    [PublicAPI]
    public class ScriptCompilationFailedException : ScriptingException
    {
        public ScriptCompilationFailedException(IScript script,
            IEnumerable<CompilationMessage> compilationResultMessages) : base("Script compilation failed")
        {
            Script = script;
            CompilationResultMessages = compilationResultMessages;
        }

        public IScript Script { get; }

        public IEnumerable<CompilationMessage> CompilationResultMessages { get; }
    }
}
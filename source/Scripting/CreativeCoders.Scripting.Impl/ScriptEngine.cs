using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.Impl
{
    [PublicAPI]
    public class ScriptEngine : IScriptEngine
    {
        public ScriptEngine(IScriptLanguage language)
        {
            Ensure.IsNotNull(language, nameof(language));

            Language = language;
        }

        public IScriptSession CreateSession(string nameSpace)
        {
            return new ScriptSession(this, nameSpace);
        }

        public IScriptLanguage Language { get; }
    }
}
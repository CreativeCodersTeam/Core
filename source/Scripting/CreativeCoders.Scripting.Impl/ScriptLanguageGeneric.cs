using CreativeCoders.CodeCompilation;
using CreativeCoders.Scripting.ClassTemplating;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.Impl
{
    [PublicAPI]
    public class ScriptLanguage<TCompiler> : ScriptLanguage
        where TCompiler : ICompiler, new()
    {
        public ScriptLanguage(string name, bool supportsDirectExecute, ScriptClassTemplate scriptClassTemplate) :
            base(name, supportsDirectExecute, scriptClassTemplate, () => new TCompiler()) { }
    }
}
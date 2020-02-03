using CreativeCoders.CodeCompilation;
using CreativeCoders.Scripting.ClassTemplating;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting
{
    [PublicAPI]
    public interface IScriptLanguage
    {
        //ISourceCodeGenerator CreateCodeGenerator();

        ICompiler CreateCompiler();

        string Name { get; }

        bool SupportsDirectExecute { get; }

        string DirectExecuteMethodName { get; }

        ScriptClassTemplate ScriptClassTemplate { get; }
    }
}
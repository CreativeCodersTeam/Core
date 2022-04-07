using System.Collections.Generic;
using CreativeCoders.CodeCompilation;
using CreativeCoders.Scripting.Base;
using CreativeCoders.Scripting.Base.Exceptions;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.CSharp.Exceptions;

[PublicAPI]
public class ScriptCompilationFailedException : ScriptingException
{
    public ScriptCompilationFailedException(ScriptPackage scriptPackage,
        IEnumerable<CompilationMessage> compilationResultMessages) : base("Script compilation failed")
    {
        ScriptPackage = scriptPackage;
        CompilationResultMessages = compilationResultMessages;
    }

    public ScriptPackage ScriptPackage { get; }

    public IEnumerable<CompilationMessage> CompilationResultMessages { get; }
}

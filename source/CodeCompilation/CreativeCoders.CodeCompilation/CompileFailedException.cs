using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.CodeCompilation;

[PublicAPI]
public class CompileFailedException : Exception
{
    public CompileFailedException(IEnumerable<CompilationMessage> compilerMessages)
    {
        CompilerMessages = compilerMessages;
    }
        
    public IEnumerable<CompilationMessage> CompilerMessages { get; }
}
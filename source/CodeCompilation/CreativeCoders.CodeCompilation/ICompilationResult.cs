using System.Collections.Generic;

namespace CreativeCoders.CodeCompilation;

public interface ICompilationResult
{
    bool Success { get; }

    IEnumerable<CompilationMessage> Messages { get; }
}

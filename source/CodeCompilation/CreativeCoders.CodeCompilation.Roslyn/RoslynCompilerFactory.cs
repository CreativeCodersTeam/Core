using JetBrains.Annotations;

namespace CreativeCoders.CodeCompilation.Roslyn;

[PublicAPI]
public class RoslynCompilerFactory : ICompilerFactory
{
    public ICompiler CreateCompiler()
    {
        return new RoslynCompiler();
    }
}

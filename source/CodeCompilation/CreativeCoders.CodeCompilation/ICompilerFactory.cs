using JetBrains.Annotations;

namespace CreativeCoders.CodeCompilation
{
    [PublicAPI]
    public interface ICompilerFactory
    {
        ICompiler CreateCompiler();
    }
}
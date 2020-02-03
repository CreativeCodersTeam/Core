namespace CreativeCoders.CodeCompilation
{
    public interface ICompiler
    {
        ICompilationResult Compile(CompilationPackage compilationPackage);
    }
}

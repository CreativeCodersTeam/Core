namespace CreativeCoders.CodeCompilation;

public class CompilationOutput
{
    public CompilationOutput(CompilationOutputKind outputKind, ICompilationOutputData outputData)
    {
        OutputKind = outputKind;
        OutputData = outputData;
    }

    public CompilationOutputKind OutputKind { get; }

    public ICompilationOutputData OutputData { get; }
}

namespace CreativeCoders.CodeCompilation;

public class SourceCodeUnit
{
    public SourceCodeUnit(string sourceCode, string fileName)
    {
        SourceCode = sourceCode;
        FileName = fileName;
    }

    public string SourceCode { get; }

    public string FileName { get; }
}
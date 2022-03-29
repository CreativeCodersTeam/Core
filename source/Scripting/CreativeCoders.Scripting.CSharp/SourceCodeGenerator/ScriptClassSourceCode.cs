namespace CreativeCoders.Scripting.CSharp.SourceCodeGenerator;

public class ScriptClassSourceCode
{
    public ScriptClassSourceCode(string nameSpace, string className, string sourceCode)
    {
        NameSpace = nameSpace;
        ClassName = className;
        SourceCode = sourceCode;
    }
        
    public string NameSpace { get; }
        
    public string ClassName { get; }
        
    public string SourceCode { get; }
}
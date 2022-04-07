namespace CreativeCoders.Scripting.CSharp.ClassTemplating;

public class ScriptClassMethod : ScriptClassMember
{
    internal ScriptClassMethod(string name, string sourceCode) : base(ScriptClassMemberType.Method, name)
    {
        SourceCode = sourceCode;
    }

    public string SourceCode { get; }
}

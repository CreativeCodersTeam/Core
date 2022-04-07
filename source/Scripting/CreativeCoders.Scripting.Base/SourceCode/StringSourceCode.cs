using JetBrains.Annotations;

namespace CreativeCoders.Scripting.Base.SourceCode;

[PublicAPI]
public class StringSourceCode : DelegateSourceCode
{
    public StringSourceCode(string sourceCode) : base(() => sourceCode) { }
}

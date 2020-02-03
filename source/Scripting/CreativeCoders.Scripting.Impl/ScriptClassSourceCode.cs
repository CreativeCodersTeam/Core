using CreativeCoders.Core;

namespace CreativeCoders.Scripting.Impl
{
    public class ScriptClassSourceCode : IScriptClassSourceCode
    {
        public ScriptClassSourceCode(IScript script, string nameSpace, string className, string sourceCode)
        {
            Ensure.IsNotNull(script, nameof(script));
            Ensure.IsNotNullOrWhitespace(className, nameof(className));
            Ensure.IsNotNull(sourceCode, nameof(sourceCode));
            Ensure.IsNotNullOrWhitespace(nameSpace, nameof(nameSpace));

            Script = script;
            ClassName = className;
            SourceCode = sourceCode;
            NameSpace = nameSpace;
        }

        public string ClassName { get; }

        public string NameSpace { get; }

        public IScript Script { get; }

        public string SourceCode { get; }
    }
}
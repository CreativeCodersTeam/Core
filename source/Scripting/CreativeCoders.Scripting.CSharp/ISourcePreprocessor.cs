using CreativeCoders.Scripting.Base;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.CSharp
{
    [PublicAPI]
    public interface ISourcePreprocessor
    {
        void Preprocess(ScriptPackage scriptPackage, CSharpScriptClassDefinition classDefinition);
    }
}
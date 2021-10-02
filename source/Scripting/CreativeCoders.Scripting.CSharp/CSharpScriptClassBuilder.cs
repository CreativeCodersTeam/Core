using System.Collections.Generic;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.Scripting.Base;

namespace CreativeCoders.Scripting.CSharp
{
    public class CSharpScriptClassBuilder
    {
        private readonly IEnumerable<ISourcePreprocessor> _sourcePreprocessors;

        public CSharpScriptClassBuilder(IEnumerable<ISourcePreprocessor> sourcePreprocessors)
        {
            _sourcePreprocessors = sourcePreprocessors;
        }
        
        public CSharpScriptClassDefinition Build(ScriptPackage scriptPackage,
            string nameSpace)
        {
            var definition = new CSharpScriptClassDefinition
            {
                SourceCode = scriptPackage.SourceCode.Read(),
                ClassName = scriptPackage.Name,
                NameSpace = nameSpace
            };

            PreprocessSourceCode(scriptPackage, definition);
            
            return definition;
        }

        private void PreprocessSourceCode(ScriptPackage scriptPackage, CSharpScriptClassDefinition classDefinition)
        {
            _sourcePreprocessors.ForEach(x => x.Preprocess(scriptPackage, classDefinition));
        }
    }
}
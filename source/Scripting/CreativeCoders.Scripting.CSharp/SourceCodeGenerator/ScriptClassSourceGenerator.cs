using CreativeCoders.Core;
using CreativeCoders.Scripting.CSharp.ClassTemplating;

namespace CreativeCoders.Scripting.CSharp.SourceCodeGenerator;

public class ScriptClassSourceGenerator
{
    private readonly ScriptClassTemplate _template;

    private readonly CSharpScriptClassDefinition _scriptClassDefinition;

    public ScriptClassSourceGenerator(ScriptClassTemplate template,
        CSharpScriptClassDefinition scriptClassDefinition)
    {
        Ensure.IsNotNull(template, nameof(template));
        Ensure.IsNotNull(scriptClassDefinition, nameof(scriptClassDefinition));

        _template = template;
        _scriptClassDefinition = scriptClassDefinition;
    }

    public ScriptClassSourceCode Generate()
    {
        var syntaxTree = new ClassSyntaxTreeBuilder(_template, _scriptClassDefinition.NameSpace,
            _scriptClassDefinition.ClassName, _scriptClassDefinition.Usings).Build();
        var sourceCode = syntaxTree.Emit(_scriptClassDefinition.SourceCode);
        var result = new ScriptClassSourceCode(_scriptClassDefinition.NameSpace,
            _scriptClassDefinition.ClassName, sourceCode);
        return result;
    }
}

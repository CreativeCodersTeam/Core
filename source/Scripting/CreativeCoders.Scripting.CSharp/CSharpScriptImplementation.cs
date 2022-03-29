using System.Collections.Generic;
using System.Reflection;
using CreativeCoders.CodeCompilation;
using CreativeCoders.Scripting.Base;
using CreativeCoders.Scripting.CSharp.ClassTemplating;
using CreativeCoders.Scripting.CSharp.Exceptions;
using CreativeCoders.Scripting.CSharp.SourceCodeGenerator;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.CSharp;

[PublicAPI]
public class CSharpScriptImplementation : IScriptRuntimeImplementation
{
    private readonly ICompiler _compiler;
        
    private readonly ScriptClassTemplate _classTemplate;

    public CSharpScriptImplementation(ScriptClassTemplate classTemplate, ICompiler compiler)
    {
        _classTemplate = classTemplate;
        _compiler = compiler;
            
        SourcePreprocessors = new List<ISourcePreprocessor>();
    }

    private Assembly CompileToAssembly(ScriptClassSourceCode classSourceCode)
    {
        var compilationPackage = BuildCompilationPackage(classSourceCode);
        compilationPackage.AddAllLoadedAssemblyReferences = true;

        return _compiler.CompileToAssembly(compilationPackage, true);
    }

    private static CompilationPackage BuildCompilationPackage(ScriptClassSourceCode classSourceCode)
    {
        var package = new CompilationPackage();
        package.SourceCodes.Add(new SourceCodeUnit(classSourceCode.SourceCode, classSourceCode.ClassName + ".cs"));
        return package;
    }

    public IScript Build(ScriptPackage scriptPackage, string nameSpace)
    {
        var classSourceCode = CreateClassSourceCode(scriptPackage, nameSpace);

        try
        {
            var assembly = CompileToAssembly(classSourceCode);
            
            return new CSharpScript(assembly, classSourceCode.NameSpace, classSourceCode.ClassName, _classTemplate.Injections);
        }
        catch (CompileFailedException e)
        {
            throw new ScriptCompilationFailedException(scriptPackage, e.CompilerMessages);
        }
    }
        
    private ScriptClassSourceCode CreateClassSourceCode(ScriptPackage scriptPackage, string nameSpace)
    {
        var classDefinition = new CSharpScriptClassBuilder(SourcePreprocessors).Build(scriptPackage, nameSpace);
        
        var classSourceCode = new ScriptClassSourceGenerator(_classTemplate, classDefinition).Generate();

        return classSourceCode;
    }
        
    public IList<ISourcePreprocessor> SourcePreprocessors { get; }
}
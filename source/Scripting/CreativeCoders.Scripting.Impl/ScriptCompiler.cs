using System.IO;
using CreativeCoders.CodeCompilation;
using CreativeCoders.Core;
using CreativeCoders.Scripting.Exceptions;

namespace CreativeCoders.Scripting.Impl
{
    internal class ScriptCompiler
    {
        private readonly ICompiler _compiler;

        private readonly IScriptClassSourceCode _scriptClassSourceCode;

        public ScriptCompiler(IScriptClassSourceCode scriptClassSourceCode, ICompiler compiler)
        {
            Ensure.IsNotNull(scriptClassSourceCode, nameof(scriptClassSourceCode));
            Ensure.IsNotNull(compiler, nameof(compiler));

            _scriptClassSourceCode = scriptClassSourceCode;
            _compiler = compiler;
        }

        public byte[] CreateAssembly()
        {
            using (var ms = new MemoryStream())
            {
                var compilationPackage = BuildCompilationPackage(_scriptClassSourceCode, ms);
                var compilationResult = _compiler.Compile(compilationPackage);
                if (compilationResult.Success)
                {
                    return ms.ToArray();
                }
                throw new ScriptCompilationFailedException(_scriptClassSourceCode.Script, compilationResult.Messages);
            }
        }

        private static CompilationPackage BuildCompilationPackage(IScriptClassSourceCode classSourceCode, Stream outputStream)
        {
            var package = new CompilationPackage
            {
                OutputKind = CompilationOutputKind.DynamicallyLinkedLibrary,
                Output = new StreamCompilationOutput(outputStream)
            };
            package.SourceCodes.Add(new SourceCodeUnit(classSourceCode.SourceCode, classSourceCode.ClassName + ".cs"));            
            return package;
        }
    }
}
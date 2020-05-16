using System;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.CodeCompilation
{
    [PublicAPI]
    public static class CompilerExtensions
    {
        public static Assembly CompileToAssembly(this ICompiler compiler, CompilationPackage compilationPackage, bool throwExceptionOnCompileFailed)
        {
            using (var memoryStream = new MemoryStream())
            {
                var output = new CompilationOutput(CompilationOutputKind.DynamicallyLinkedLibrary, new StreamCompilationOutputData(memoryStream));
            
                var compilerResult = compiler.Compile(compilationPackage, output);

                if (compilerResult.Success)
                {
                    return Assembly.Load(memoryStream.ToArray());
                }
                
                if (throwExceptionOnCompileFailed)
                {
                    throw new CompileFailedException(compilerResult.Messages);
                }
            }

            return null;
        }

        public static T CreateScriptObject<T>(this ICompiler compiler, CompilationPackage compilationPackage, string typeName, bool throwExceptionOnCompileFailed)
            where T : class
        {
            var assembly = compiler.CompileToAssembly(compilationPackage, throwExceptionOnCompileFailed);

            return assembly.CreateInstance(typeName) as T;
        }
        
        public static T CreateScriptObject<T>(this ICompiler compiler, CompilationPackage compilationPackage, bool throwExceptionOnCompileFailed)
            where T : class
        {
            var assembly = compiler.CompileToAssembly(compilationPackage, throwExceptionOnCompileFailed);

            var type = assembly.ExportedTypes.FirstOrDefault();

            if (type == null)
            {
                return null;
            }

            return Activator.CreateInstance(type) as T;
        }
    }
}
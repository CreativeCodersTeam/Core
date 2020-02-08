using System.Collections.Generic;
using System.Reflection;

namespace CreativeCoders.CodeCompilation
{
    public class CompilationPackage
    {
        public CompilationPackage()
        {
            ReferencedAssemblyFiles = new List<string>();
            SourceCodes = new List<SourceCodeUnit>();
        }

        public void AddReferenceAssembly(Assembly assembly)
        {
            ReferencedAssemblyFiles.Add(assembly.Location);
        }

        public string AssemblyName { get; set; }

        public IList<SourceCodeUnit> SourceCodes { get; }

        public ICompilationOutput Output { get; set; }

        public IList<string> ReferencedAssemblyFiles { get; }

        public CompilationOutputKind OutputKind { get; set; }
    }
}
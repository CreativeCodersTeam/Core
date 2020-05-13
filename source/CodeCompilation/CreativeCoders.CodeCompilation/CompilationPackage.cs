using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.CodeCompilation
{
    [PublicAPI]
    public class CompilationPackage
    {
        public CompilationPackage()
        {
            AddNetStandardReferences = true;
            ReferencedAssemblyFiles = new List<string>();
            SourceCodes = new List<SourceCodeUnit>();
        }

        public void AddReferenceAssembly(Assembly assembly)
        {
            ReferencedAssemblyFiles.Add(assembly.Location);
        }

        public string AssemblyName { get; set; }

        public IList<SourceCodeUnit> SourceCodes { get; }

        public IList<string> ReferencedAssemblyFiles { get; }

        public bool AddNetStandardReferences { get; set; }
        
        public bool AddAllLoadedAssemblyReferences { get; set; }
    }
}
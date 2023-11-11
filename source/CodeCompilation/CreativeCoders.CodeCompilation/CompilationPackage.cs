using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.CodeCompilation;

[PublicAPI]
public class CompilationPackage
{
    public void AddReferenceAssembly(Assembly assembly)
    {
        ReferencedAssemblyFiles.Add(assembly.Location);
    }

    public string AssemblyName { get; set; }

    public IList<SourceCodeUnit> SourceCodes { get; } = new List<SourceCodeUnit>();

    public IList<string> ReferencedAssemblyFiles { get; } = new List<string>();

    public bool AddNetStandardReferences { get; set; } = true;

    public bool AddAllLoadedAssemblyReferences { get; set; }
}

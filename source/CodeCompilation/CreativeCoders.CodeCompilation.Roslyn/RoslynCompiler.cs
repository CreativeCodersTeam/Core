using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.IO;
using CreativeCoders.Core.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CreativeCoders.CodeCompilation.Roslyn
{
    public class RoslynCompiler : ICompiler
    {
        public ICompilationResult Compile(CompilationPackage compilationPackage, CompilationOutput compilationOutput)
        {
            Ensure.IsNotNull(compilationOutput, nameof(compilationOutput));

            var syntaxTrees = CreateSyntaxTrees(compilationPackage.SourceCodes).ToArray();

            var assemblyFiles =
                GetAssembliesFromUsings(
                    syntaxTrees.SelectMany(syntaxTree =>
                        syntaxTree.GetCompilationUnitRoot().Usings.Select(usingDirective => usingDirective)),
                    compilationPackage);

            var compilation = CreateCompilation(syntaxTrees, assemblyFiles, compilationPackage, compilationOutput);

            return new RoslynCompilationResult(compilation, compilationOutput.OutputData);
        }

        private static IEnumerable<string> GetAssembliesFromUsings(IEnumerable<UsingDirectiveSyntax> usings,
            CompilationPackage compilationPackage)
        {
            var assemblies = new List<string> {typeof(object).Assembly.Location};
            assemblies.AddRange(compilationPackage.ReferencedAssemblyFiles);

            if (compilationPackage.AddAllLoadedAssemblyReferences)
            {
                var loadedAssemblies = ReflectionUtils.GetAllAssemblies();
                usings.ForEach(usingDirective => GetAssemblyForUsing(usingDirective, loadedAssemblies, assemblies));
            }

            if (compilationPackage.AddNetStandardReferences)
            {
                assemblies.AddRange(GetNetStandardAssemblies());
            }

            return assemblies.Distinct();
        }

        private static IEnumerable<string> GetNetStandardAssemblies()
        {
            var netStdAssembly = Assembly.Load(new AssemblyName("netstandard"));
            
            var referencedAssemblyNames = netStdAssembly.GetReferencedAssemblies();
            
            var assemblyLocations = referencedAssemblyNames.Select(Assembly.Load)
                .Select(assembly => assembly.Location).ToList();
            assemblyLocations.Add(netStdAssembly.Location);
            return assemblyLocations;
        }

        private static void GetAssemblyForUsing(UsingDirectiveSyntax usingDirectiveSyntax, IEnumerable<Assembly> loadedAssemblies, ICollection<string> assemblies)
        {
            var nameSpace = usingDirectiveSyntax.Name.GetText().ToString();

            var foundAssemblies = loadedAssemblies.Where(assembly => !assembly.IsDynamic &&
                assembly.GetExportedTypes().Any(type => type.Namespace?.Equals(nameSpace) == true));
            
            foundAssemblies.ForEach(assembly => assemblies.Add(assembly.Location));
        }

        private static IEnumerable<MetadataReference> GetReferences(IEnumerable<string> assemblyFiles)
        {
            return assemblyFiles.Distinct().Select(assemblyFile => MetadataReference.CreateFromFile(assemblyFile));
        }

        private static CSharpCompilation CreateCompilation(IEnumerable<SyntaxTree> syntaxTrees,
            IEnumerable<string> assemblyFiles,
            CompilationPackage compilationPackage, CompilationOutput compilationOutput)
        {
            var assemblyName = compilationPackage.AssemblyName;
            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                assemblyName = FileSys.Path.ChangeExtension(FileSys.Path.GetRandomFileName(), ".dll");
            }

            var references = GetReferences(assemblyFiles);

            var compilation = CSharpCompilation.Create(assemblyName, syntaxTrees, references,
                new CSharpCompilationOptions(RoslynConvert.ConvertOutputKind(compilationOutput.OutputKind)));

            return compilation;
        }

        private static IEnumerable<CSharpSyntaxTree> CreateSyntaxTrees(IEnumerable<SourceCodeUnit> sourceCodeUnits)
        {
            return sourceCodeUnits.Select(CreateSyntaxTree);
        }

        private static CSharpSyntaxTree CreateSyntaxTree(SourceCodeUnit sourceCodeUnit)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCodeUnit.SourceCode, null, sourceCodeUnit.FileName) as CSharpSyntaxTree;
            return syntaxTree;
        }
    }
}
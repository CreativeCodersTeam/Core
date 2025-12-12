using System.Reflection;

namespace CreativeCoders.Cli.Hosting.Commands;

public interface IAssemblyCommandScanner
{
    AssemblyScanResult ScanForCommands(Assembly[] assemblies, Func<Type, bool>? predicate = null);
}

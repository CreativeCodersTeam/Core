using System.Reflection;

namespace CreativeCoders.Cli.Hosting.Commands;

public interface IAssemblyCommandScanner
{
    IEnumerable<CliCommandInfo> Scan(IEnumerable<Assembly> assemblies);
}

using System.Reflection;

namespace CreativeCoders.Cli.Hosting.Commands;

/// <summary>
/// Defines a scanner that discovers CLI commands by scanning assemblies.
/// </summary>
public interface IAssemblyCommandScanner
{
    /// <summary>
    /// Scans the specified assemblies for CLI commands decorated with <see cref="CliCommandAttribute"/>.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan for commands.</param>
    /// <param name="predicate">An optional predicate to filter discovered command types.</param>
    /// <returns>An <see cref="AssemblyScanResult"/> containing the discovered commands and group attributes.</returns>
    AssemblyScanResult ScanForCommands(Assembly[] assemblies, Func<Type, bool>? predicate = null);
}

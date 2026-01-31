using System.Reflection;
using CreativeCoders.Cli.Core;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;

namespace CreativeCoders.Cli.Hosting.Commands;

public class AssemblyCommandScanner(ICommandInfoCreator commandInfoCreator) : IAssemblyCommandScanner
{
    private readonly ICommandInfoCreator _commandInfoCreator = Ensure.NotNull(commandInfoCreator);

    public AssemblyScanResult ScanForCommands(Assembly[] assemblies, Func<Type, bool>? predicate = null)
    {
        var commandInfos = assemblies
            .SelectMany(x => x.GetTypesSafe())
            .Where(x => x.GetCustomAttributes(typeof(CliCommandAttribute), false).Length != 0)
            .Select(x => _commandInfoCreator.Create(x))
            .Where(x => x != null && (predicate == null || predicate(x.CommandType)))
            .OfType<CliCommandInfo>()
            .ToArray();

        var groupAttributes = assemblies
            .SelectMany(x => x.GetCustomAttributes<CliCommandGroupAttribute>());

        return new AssemblyScanResult
        {
            CommandInfos = commandInfos,
            GroupAttributes = groupAttributes
        };
    }
}

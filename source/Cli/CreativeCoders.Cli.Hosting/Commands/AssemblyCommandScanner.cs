using System.Reflection;
using CreativeCoders.Cli.Core;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;

namespace CreativeCoders.Cli.Hosting.Commands;

public class AssemblyCommandScanner(ICommandInfoCreator commandInfoCreator) : IAssemblyCommandScanner
{
    private readonly ICommandInfoCreator _commandInfoCreator = Ensure.NotNull(commandInfoCreator);

    public IEnumerable<CliCommandInfo> Scan(IEnumerable<Assembly> assemblies)
    {
        return assemblies
            .SelectMany(x => x.GetTypesSafe())
            .Where(x => x.GetCustomAttributes(typeof(CliCommandAttribute), false).Length != 0)
            .Select(x => _commandInfoCreator.Create(x))
            .Where(x => x != null)
            .OfType<CliCommandInfo>();
    }
}

using System.Reflection;
using CreativeCoders.Cli.Core;
using CreativeCoders.Core.Reflection;

namespace CreativeCoders.Cli.Hosting.Commands;

public class AssemblyCommandScanner : IAssemblyCommandScanner
{
    public IEnumerable<CliCommandInfo> Scan(IEnumerable<Assembly> assemblies)
    {
        return assemblies
            .SelectMany(x => x.GetTypesSafe())
            .Where(x => x.GetCustomAttributes(typeof(CliCommandAttribute), false).Length != 0)
            .Select(x => new CliCommandInfo
                { CommandAttribute = x.GetCustomAttribute<CliCommandAttribute>(false)!, CommandType = x });
    }
}

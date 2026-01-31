using CreativeCoders.Cli.Core;

namespace CreativeCoders.Cli.Hosting.Commands;

public class AssemblyScanResult
{
    public required IEnumerable<CliCommandInfo> CommandInfos { get; init; }

    public required IEnumerable<CliCommandGroupAttribute> GroupAttributes { get; init; }
}

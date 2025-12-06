using CreativeCoders.Cli.Core;

namespace CreativeCoders.Cli.Hosting.Commands;

public class CliCommandInfo
{
    public required CliCommandAttribute CommandAttribute { get; init; }

    public required Type CommandType { get; init; }
}
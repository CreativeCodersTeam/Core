using CreativeCoders.Core;

namespace CreativeCoders.Cli.Core;

[AttributeUsage(AttributeTargets.Class)]
public class CliCommandAttribute(string[] commands) : Attribute
{
    public string Name { get; set; } = string.Empty;

    public string[] Commands { get; } = Ensure.NotNull(commands);

    public string Description { get; set; } = string.Empty;

    public string[] AlternativeCommands { get; set; } = [];

    public bool IsDefaultCommand { get; set; }
}

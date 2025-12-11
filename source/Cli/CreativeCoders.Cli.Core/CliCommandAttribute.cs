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

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
public class CliCommandGroupAttribute(string[] commands, string description) : Attribute
{
    public string[] Commands { get; } = commands;

    public string Description { get; } = description;
}

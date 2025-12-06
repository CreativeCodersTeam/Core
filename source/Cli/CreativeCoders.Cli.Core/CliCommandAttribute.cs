namespace CreativeCoders.Cli.Core;

[AttributeUsage(AttributeTargets.Class)]
public class CliCommandAttribute : Attribute
{
    public string[] Commands { get; }

    public CliCommandAttribute(string[] commands)
    {
        Commands = commands;
    }

    public string Description { get; set; } = string.Empty;

    public string[][] AlternativeCommands { get; set; }

    public bool IsDefaultCommand { get; set; }
}

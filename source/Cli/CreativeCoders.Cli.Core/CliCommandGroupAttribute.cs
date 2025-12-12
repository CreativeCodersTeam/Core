namespace CreativeCoders.Cli.Core;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class CliCommandGroupAttribute(string[] commands, string description) : Attribute
{
    public string[] Commands { get; } = commands;

    public string Description { get; } = description;
}

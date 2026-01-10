using CreativeCoders.Core;

namespace CreativeCoders.CakeBuild;

public class ToolInstallation(string toolKind, string name, string version)
{
    public string ToolKind { get; set; } = Ensure.IsNotNullOrWhitespace(toolKind);

    public string Name { get; set; } = Ensure.IsNotNullOrWhitespace(name);

    public string Version { get; set; } = Ensure.IsNotNullOrWhitespace(version);
}
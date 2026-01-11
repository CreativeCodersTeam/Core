using CreativeCoders.Core;

namespace CreativeCoders.CakeBuild;

public class ToolInstallation(string toolKind, string name, string version)
{
    public string ToolKind { get; } = Ensure.IsNotNullOrWhitespace(toolKind);

    public string Name { get; } = Ensure.IsNotNullOrWhitespace(name);

    public string Version { get; } = Ensure.IsNotNullOrWhitespace(version);
}

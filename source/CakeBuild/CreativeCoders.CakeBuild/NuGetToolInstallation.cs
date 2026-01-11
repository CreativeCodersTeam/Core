using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild;

[PublicAPI]
public class NuGetToolInstallation(string name, string version) : ToolInstallation("nuget", name, version);

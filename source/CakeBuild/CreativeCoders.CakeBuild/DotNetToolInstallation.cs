using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild;

[PublicAPI]
public class DotNetToolInstallation(string name, string version) : ToolInstallation("dotnet", name, version);

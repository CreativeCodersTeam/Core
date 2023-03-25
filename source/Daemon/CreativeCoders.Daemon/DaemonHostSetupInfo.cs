using System;

namespace CreativeCoders.Daemon;

internal class DaemonHostSetupInfo
{
    public string[]? Args { get; set; }

    public Type? InstallerType { get; set; }

    public string InstallArg { get; set; } = "--install";

    public string UninstallArg { get; set; } = "--uninstall";

    public string? DefinitionFileName { get; set; }
}

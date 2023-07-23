using System.Diagnostics;
using System.Reflection;
using System.Text;
using CreativeCoders.Core.Text;
using CreativeCoders.Core.IO;
using CreativeCoders.Daemon.Definition;
using JetBrains.Annotations;

namespace CreativeCoders.Daemon.Linux;

/// <summary>   A systemd daemon installer. </summary>
[UsedImplicitly]
public class SystemdServiceInstaller : IDaemonInstaller
{
    private const string SystemdConfigPath = "/etc/systemd/system";

    /// <summary>
    /// Installs a systemd service based daemon definition.
    /// </summary>
    /// <param name="daemonDefinition">The definition to install</param>
    public void Install(DaemonDefinition daemonDefinition)
    {
        var configContent = CreateServiceConfig(daemonDefinition);

        var serviceName = daemonDefinition.Name + ".service";

        var serviceConfigFileName = FileSys.Path.Combine(SystemdConfigPath, serviceName);
        FileSys.File.WriteAllText(serviceConfigFileName, configContent);

        RunAndWaitSystemctl("enable", serviceName);

        RunAndWaitSystemctl("start", serviceName);
    }

    /// <summary>
    /// Uninstalls the systemd service specified in the daemon definition.
    /// </summary>
    /// <param name="daemonDefinition"></param>
    public void Uninstall(DaemonDefinition daemonDefinition)
    {
        var serviceName = daemonDefinition.Name + ".service";

        RunAndWaitSystemctl("stop", serviceName);
        RunAndWaitSystemctl("disable", serviceName);
        RunAndWait("rm", FileSys.Path.Combine(SystemdConfigPath, serviceName));
        RunAndWaitSystemctl("daemon-reload");
        RunAndWaitSystemctl("reset-failed");
    }

    private static void RunAndWaitSystemctl(params string[] args)
    {
        RunAndWait("systemctl", args);
    }

    private static void RunAndWait(string command, params string[] args)
    {
        Process.Start(command, string.Join(" ", args)).WaitForExit();
    }

    private static string CreateServiceConfig(DaemonDefinition daemonDefinition)
    {
        var serviceConfig = new StringBuilder();

        if (string.IsNullOrWhiteSpace(daemonDefinition.WorkingDirectory))
        {
            daemonDefinition.WorkingDirectory =
                FileSys.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)
                ?? string.Empty;
        }

        serviceConfig
            .AppendLine("[Unit]")
            .AppendLine($"Description={daemonDefinition.Description}")
            .AppendLine($"Requires={string.Join(" ", daemonDefinition.DaemonsDependedOn)}",
                !(daemonDefinition.DaemonsDependedOn.Length > 1))
            .AppendLine($"After={string.Join(" ", daemonDefinition.StartAfterDaemons)}",
                !(daemonDefinition.StartAfterDaemons.Length > 1))
            .AppendLine($"Before={string.Join(" ", daemonDefinition.StartBeforeDaemons)}",
                !(daemonDefinition.StartBeforeDaemons.Length > 1))
            .AppendLine("")
            .AppendLine("[Service]")
            .AppendLine("Type=notify")
            .AppendLine($"WorkingDirectory={daemonDefinition.WorkingDirectory}")
            .AppendLine($"ExecStart=/usr/bin/dotnet {Assembly.GetEntryAssembly()?.Location}")
            .AppendLine("Restart=always")
            .AppendLine("RestartSec=10")
            .AppendLine($"SyslogIdentifier={daemonDefinition.SyslogIdentifier}",
                !string.IsNullOrWhiteSpace(daemonDefinition.SyslogIdentifier))
            .AppendLine($"User={daemonDefinition.User}", string.IsNullOrWhiteSpace(daemonDefinition.User))
            .AppendLine($"Environment={daemonDefinition.Environment}",
                string.IsNullOrWhiteSpace(daemonDefinition.Environment))
            .AppendLine()
            .AppendLine("[Install]")
            .AppendLine("WantedBy=multi-user.target");

        return serviceConfig.ToString();
    }
}

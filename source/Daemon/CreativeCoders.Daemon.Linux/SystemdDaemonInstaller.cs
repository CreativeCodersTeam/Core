using System.Diagnostics;
using System.Reflection;
using System.Text;
using CreativeCoders.Core.IO;
using CreativeCoders.Core.SysEnvironment;
using CreativeCoders.Core.Text;
using CreativeCoders.Daemon.Base.Info;

namespace CreativeCoders.Daemon.Linux;

/// <summary>   A systemd daemon installer. </summary>
public class SystemdDaemonInstaller
{
    private const string SystemdConfigPath = "/etc/systemd/system";

    private readonly DaemonInfo _daemonInfo;

    private readonly string _serviceName;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="SystemdDaemonInstaller"/> class.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public SystemdDaemonInstaller()
    {
        _daemonInfo = LoadDaemonInfo();
        _serviceName = _daemonInfo.Name + ".service";
    }

    private static DaemonInfo LoadDaemonInfo()
    {
        var path = Env.GetAppDirectory();
        var fileName = FileSys.Path.Combine(path, "daemon.json");
        var infoFile = new DaemonInfoFile(fileName);
        return infoFile.LoadInfo();
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Installs a systemd service based on the daemon.json file in the app directory.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public void Install()
    {
        var configContent = CreateServiceConfig();

        var serviceConfigFileName = FileSys.Path.Combine(SystemdConfigPath, _serviceName);
        FileSys.File.WriteAllText(serviceConfigFileName, configContent);
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Uninstalls the systemd service specified in the daemon.json file in the app directory.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public void Uninstall()
    {
        RunAndWaitSystemctl("stop", _serviceName);
        RunAndWaitSystemctl("disable", _serviceName);
        RunAndWait("rm", FileSys.Path.Combine(SystemdConfigPath, _serviceName));
        RunAndWaitSystemctl("daemon-reload");
        RunAndWaitSystemctl("reset-failed");
    }

    private static void RunAndWaitSystemctl(params string[] args)
    {
        RunAndWait("systemctl", args);
    }

    private static void RunAndWait(string command, params string[] args)
    {
        Process.Start(command, string.Join(" ", args))?.WaitForExit();
    }

    private string CreateServiceConfig()
    {
        var serviceConfig = new StringBuilder();

        if (string.IsNullOrWhiteSpace(_daemonInfo.WorkingDirectory))
        {
            _daemonInfo.WorkingDirectory =
                FileSys.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        }

        serviceConfig
            .AppendLine("[Unit]")
            .AppendLine($"Description={_daemonInfo.Description}")
            .AppendLine($"Requires={string.Join(" ", _daemonInfo.DaemonsDependedOn)}",
                !(_daemonInfo.DaemonsDependedOn?.Length > 1))
            .AppendLine($"After={string.Join(" ", _daemonInfo.StartAfterDaemons)}",
                !(_daemonInfo.StartAfterDaemons?.Length > 1))
            .AppendLine($"Before={string.Join(" ", _daemonInfo.StartBeforeDaemons)}",
                !(_daemonInfo.StartBeforeDaemons?.Length > 1))
            .AppendLine("")
            .AppendLine("[Service]")
            .AppendLine($"WorkingDirectory={_daemonInfo.WorkingDirectory}")
            .AppendLine($"ExecStart=/usr/bin/dotnet {Assembly.GetEntryAssembly()?.Location}")
            .AppendLine("Restart=always")
            .AppendLine("RestartSec=10")
            .AppendLine($"SyslogIdentifier={_daemonInfo.SyslogIdentifier}")
            .AppendLine($"User={_daemonInfo.User}", string.IsNullOrWhiteSpace(_daemonInfo.User))
            .AppendLine($"Environment={_daemonInfo.Environment}",
                string.IsNullOrWhiteSpace(_daemonInfo.Environment))
            .AppendLine()
            .AppendLine("[Install]")
            .AppendLine("WantedBy=multi-user.target");

        return serviceConfig.ToString();
    }
}

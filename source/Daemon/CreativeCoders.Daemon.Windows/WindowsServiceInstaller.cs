using System.Diagnostics;
using System.Text;
using CreativeCoders.Core.SysEnvironment;
using CreativeCoders.Core.Text;
using CreativeCoders.Daemon.Definition;
using JetBrains.Annotations;

namespace CreativeCoders.Daemon.Windows;

/// <summary> The windows service daemon installer. </summary>
[UsedImplicitly]
public class WindowsServiceInstaller : IDaemonInstaller
{
    /// <summary> Installs a windows service based on the daemon.json file in the app directory. </summary>
    public void Install(DaemonDefinition daemonDefinition)
    {
        var startMode = GetStartMode(daemonDefinition);
        var user = GetUser(daemonDefinition);
        var binPath = $"{Env.GetAppFileName()} {string.Join(" ", daemonDefinition.Arguments)}".Trim();

        var args = new StringBuilder();
        args
            .Append($"create {daemonDefinition.Name} binPath= \"{binPath}\"")
            .AppendIf(!string.IsNullOrWhiteSpace(daemonDefinition.DisplayName),
                $" DisplayName= \"{daemonDefinition.DisplayName}\"")
            .AppendIf(!string.IsNullOrWhiteSpace(startMode), $" start= {startMode}")
            .AppendIf(!string.IsNullOrEmpty(user), $" obj= {user}")
            .AppendIf(!string.IsNullOrEmpty(daemonDefinition.Password), $" password= \"{daemonDefinition.Password}\"")
            .AppendIf(daemonDefinition.DaemonsDependedOn.Length > 0,
                $" depend= {string.Join("/", daemonDefinition.DaemonsDependedOn)}");

        var startInfo = new ProcessStartInfo("sc.exe", args.ToString()) {UseShellExecute = false};

        var process = Process.Start(startInfo);

        process?.WaitForExit();

        if (process is not {ExitCode: 0})
        {
            Console.WriteLine("Service creation failed!");
        }
    }

    private static string GetStartMode(DaemonDefinition daemonDefinition)
    {
        return daemonDefinition.StartMode switch
        {
            DaemonStartMode.Boot => "boot",
            DaemonStartMode.System => "system",
            DaemonStartMode.Automatic => daemonDefinition.DelayedAutoStart ? "delayed-auto" : "auto",
            DaemonStartMode.Manual => "demand",
            DaemonStartMode.Disabled => "disabled",
            _ => throw new ArgumentOutOfRangeException(nameof(DaemonDefinition.StartMode),
                "Daemon start mode is unknown")
        };
    }

    private static string GetUser(DaemonDefinition daemonDefinition)
    {
        return daemonDefinition.Account switch
        {
            DaemonAccount.LocalService => "NT AUTHORITY\\LocalService",
            DaemonAccount.LocalSystem => string.Empty,
            DaemonAccount.NetworkService => "NT AUTHORITY\\NetworkService",
            DaemonAccount.User => daemonDefinition.User,
            _ => throw new ArgumentOutOfRangeException(nameof(DaemonDefinition.Account),
                "Daemon account is unknown")
        };
    }

    /// <summary>
    ///     Uninstalls the windows service specified in the daemon.json file in the app directory.
    /// </summary>
    public void Uninstall(DaemonDefinition daemonDefinition)
    {
        var startInfo = new ProcessStartInfo("sc.exe", $"delete {daemonDefinition.Name}");

        Process.Start(startInfo)?.WaitForExit();
    }
}

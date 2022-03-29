using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using CreativeCoders.Core.IO;
using CreativeCoders.Core.SysEnvironment;
using CreativeCoders.Core.Text;
using CreativeCoders.Daemon.Base.Info;

namespace CreativeCoders.Daemon.Windows;

/// <summary>   The windows service daemon installer. </summary>
public class WindowsServiceInstaller
{
    private readonly DaemonInfo _daemonInfo;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="WindowsServiceInstaller"/> class.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public WindowsServiceInstaller()
    {
        _daemonInfo = LoadDaemonInfo();
    }

    private static DaemonInfo LoadDaemonInfo()
    {
        var path = FileSys.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty;
        var fileName = FileSys.Path.Combine(path, "daemon.json");
        var infoFile = new DaemonInfoFile(fileName);
        return infoFile.LoadInfo();
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Installs a windows service based on the daemon.json file in the app directory.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public void Install()
    {
        var startMode = GetStartMode();
        var user = GetUser();
        var binPath = $"{Env.GetAppFileName()} {string.Join(" ", _daemonInfo.Arguments)}".Trim();

        var args = new StringBuilder();
        args
            .Append($"create {_daemonInfo.Name} binPath= \"{binPath}\"")
            .AppendIf(!string.IsNullOrWhiteSpace(_daemonInfo.DisplayName),
                $" DisplayName= \"{_daemonInfo.DisplayName}\"")
            .AppendIf(!string.IsNullOrWhiteSpace(startMode), $" start= {startMode}")
            .AppendIf(!string.IsNullOrEmpty(user), $" obj= {user}")
            .AppendIf(!string.IsNullOrEmpty(_daemonInfo.Password), $" password= \"{_daemonInfo.Password}\"")
            .AppendIf(_daemonInfo.DaemonsDependedOn.Length > 0, $" depend= {string.Join("/", _daemonInfo.DaemonsDependedOn)}");

        var startInfo = new ProcessStartInfo("sc.exe", args.ToString()) {UseShellExecute = false};

        var process = Process.Start(startInfo);

        process?.WaitForExit();

        if (process is not { ExitCode: 0 })
        {
            Console.WriteLine("Service creation failed!");
        }
    }

    private string GetStartMode()
    {
        return _daemonInfo.StartMode switch
        {
            DaemonStartMode.Boot => "boot",
            DaemonStartMode.System => "system",
            DaemonStartMode.Automatic => _daemonInfo.DelayedAutoStart ? "delayed-auto" : "auto",
            DaemonStartMode.Manual => "demand",
            DaemonStartMode.Disabled => "disabled",
            _ => throw new ArgumentOutOfRangeException(nameof(DaemonInfo.StartMode), "Daemon start mode is unknown")
        };
    }

    private string GetUser()
    {
        return _daemonInfo.Account switch
        {
            DaemonAccount.LocalService => "NT AUTHORITY\\LocalService",
            DaemonAccount.LocalSystem => string.Empty,
            DaemonAccount.NetworkService => "NT AUTHORITY\\NetworkService",
            DaemonAccount.User => _daemonInfo.User,
            _ => throw new ArgumentOutOfRangeException(nameof(DaemonInfo.Account), "Daemon account is unknown")
        };
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Uninstalls the windows service specified in the daemon.json file in the app directory.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public void Uninstall()
    {
        var startInfo = new ProcessStartInfo("sc.exe", $"delete {_daemonInfo.Name}");

        Process.Start(startInfo)?.WaitForExit();
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.SysEnvironment;

[PublicAPI]
[ExcludeFromCodeCoverage]
public static class Env
{
    private static IEnvironment __environmentInstance = new EnvironmentWrapper();

    public static IDisposable SetEnvironmentImpl(IEnvironment environment)
    {
        Ensure.IsNotNull(environment);

        var oldEnvironment = __environmentInstance;

        var resetEnvImpl = new DelegateDisposable(() => __environmentInstance = oldEnvironment, true);

        __environmentInstance = environment;

        return resetEnvImpl;
    }

    public static void Exit(int exitCode)
    {
        __environmentInstance.Exit(exitCode);
    }

    public static string ExpandEnvironmentVariables(string name)
    {
        return __environmentInstance.ExpandEnvironmentVariables(name);
    }

    public static void FailFast(string? message)
    {
        __environmentInstance.FailFast(message);
    }

    public static void FailFast(string? message, Exception? exception)
    {
        __environmentInstance.FailFast(message, exception);
    }

    public static string[] GetCommandLineArgs()
    {
        return __environmentInstance.GetCommandLineArgs();
    }

    public static string? GetEnvironmentVariable(string variable)
    {
        return __environmentInstance.GetEnvironmentVariable(variable);
    }

    public static string? GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
    {
        return __environmentInstance.GetEnvironmentVariable(variable, target);
    }

    public static IDictionary<string, object?> GetEnvironmentVariables()
    {
        return __environmentInstance.GetEnvironmentVariables();
    }

    public static IDictionary<string, object?> GetEnvironmentVariables(EnvironmentVariableTarget target)
    {
        return __environmentInstance.GetEnvironmentVariables(target);
    }

    public static string GetFolderPath(Environment.SpecialFolder folder)
    {
        return __environmentInstance.GetFolderPath(folder);
    }

    public static string GetFolderPath(Environment.SpecialFolder folder,
        Environment.SpecialFolderOption option)
    {
        return __environmentInstance.GetFolderPath(folder, option);
    }

    public static string[] GetLogicalDrives()
    {
        return __environmentInstance.GetLogicalDrives();
    }

    public static void SetEnvironmentVariable(string variable, string? value)
    {
        __environmentInstance.SetEnvironmentVariable(variable, value);
    }

    public static void SetEnvironmentVariable(string variable, string? value,
        EnvironmentVariableTarget target)
    {
        __environmentInstance.SetEnvironmentVariable(variable, value, target);
    }

    public static string? GetAppDirectory()
    {
        return __environmentInstance.GetAppDirectory();
    }

    public static string GetAppFileName()
    {
        return __environmentInstance.GetAppFileName();
    }

    public static string CommandLine => __environmentInstance.CommandLine;

    public static string CurrentDirectory
    {
        get => __environmentInstance.CurrentDirectory;
        set => __environmentInstance.CurrentDirectory = value;
    }

    public static int CurrentManagedThreadId => __environmentInstance.CurrentManagedThreadId;

    public static int ExitCode
    {
        get => __environmentInstance.ExitCode;
        set => __environmentInstance.ExitCode = value;
    }

    public static bool HasShutdownStarted => __environmentInstance.HasShutdownStarted;

    public static bool Is64BitOperatingSystem => __environmentInstance.Is64BitOperatingSystem;

    public static bool Is64BitProcess => __environmentInstance.Is64BitProcess;

    public static string MachineName => __environmentInstance.MachineName;

    public static string NewLine => __environmentInstance.NewLine;

    public static OperatingSystem OSVersion => __environmentInstance.OSVersion;

    public static int ProcessorCount => __environmentInstance.ProcessorCount;

    public static string StackTrace => __environmentInstance.StackTrace;

    public static string SystemDirectory => __environmentInstance.SystemDirectory;

    public static int SystemPageSize => __environmentInstance.SystemPageSize;

    public static int TickCount => __environmentInstance.TickCount;

    public static string UserDomainName => __environmentInstance.UserDomainName;

    public static bool UserInteractive => __environmentInstance.UserInteractive;

    public static string UserName => __environmentInstance.UserName;

    public static Version Version => __environmentInstance.Version;

    public static long WorkingSet => __environmentInstance.WorkingSet;
}

using System;
using System.Collections.Generic;

#nullable enable

namespace CreativeCoders.Core.SysEnvironment;

public interface IEnvironment
{
    void Exit(int exitCode);

    string ExpandEnvironmentVariables(string name);

    void FailFast(string? message);

    void FailFast(string? message, Exception? exception);

    string[] GetCommandLineArgs();

    string? GetEnvironmentVariable(string variable);

    string? GetEnvironmentVariable(string variable, EnvironmentVariableTarget target);

    IDictionary<string, object?> GetEnvironmentVariables();

    IDictionary<string, object?> GetEnvironmentVariables(EnvironmentVariableTarget target);

    string GetFolderPath(Environment.SpecialFolder folder);

    string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option);

    string[] GetLogicalDrives();

    void SetEnvironmentVariable(string variable, string? value);

    void SetEnvironmentVariable(string variable, string? value, EnvironmentVariableTarget target);

    string GetAppDirectory();

    string GetAppFileName();

    string CommandLine { get; }

    string CurrentDirectory { get; set; }

    int CurrentManagedThreadId { get; }

    int ExitCode { get; set; }

    bool HasShutdownStarted { get; }

    bool Is64BitOperatingSystem { get; }

    bool Is64BitProcess { get; }

    string MachineName { get; }

    string NewLine { get; }

    OperatingSystem OSVersion { get; }

    int ProcessorCount { get; }

    string StackTrace { get; }

    string SystemDirectory { get; }

    int SystemPageSize { get; }

    int TickCount { get; }

    string UserDomainName { get; }

    bool UserInteractive { get; }

    string UserName { get; }

    Version Version { get; }

    long WorkingSet { get; }
}

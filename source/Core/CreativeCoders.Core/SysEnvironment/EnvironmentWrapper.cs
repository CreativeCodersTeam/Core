using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Core.IO;

#nullable enable

namespace CreativeCoders.Core.SysEnvironment;

/// <summary>
/// Default implementation of <see cref="IEnvironment"/> that delegates all calls to <see cref="Environment"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public class EnvironmentWrapper : IEnvironment
{
    /// <inheritdoc/>
    public void Exit(int exitCode)
    {
        Environment.Exit(exitCode);
    }

    /// <inheritdoc/>
    public string ExpandEnvironmentVariables(string name)
    {
        return Environment.ExpandEnvironmentVariables(name);
    }

    /// <inheritdoc/>
    public void FailFast(string? message)
    {
        Environment.FailFast(message);
    }

    /// <inheritdoc/>
    public void FailFast(string? message, Exception? exception)
    {
        Environment.FailFast(message, exception);
    }

    /// <inheritdoc/>
    public string[] GetCommandLineArgs()
    {
        return Environment.GetCommandLineArgs();
    }

    /// <inheritdoc/>
    public string? GetEnvironmentVariable(string variable)
    {
        return Environment.GetEnvironmentVariable(variable);
    }

    /// <inheritdoc/>
    public string? GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
    {
        return Environment.GetEnvironmentVariable(variable, target);
    }

    /// <inheritdoc/>
    public IDictionary<string, object?> GetEnvironmentVariables()
    {
        var variables = new Dictionary<string, object?>();

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables())
        {
            var key = environmentVariable.Key.ToString();

            if (key == null)
            {
                continue;
            }

            variables.Add(key, environmentVariable.Value);
        }

        return variables;
    }

    /// <inheritdoc/>
    public IDictionary<string, object?> GetEnvironmentVariables(EnvironmentVariableTarget target)
    {
        var variables = new Dictionary<string, object?>();

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables(target))
        {
            var key = environmentVariable.Key.ToString();

            if (key == null)
            {
                continue;
            }

            variables.Add(key, environmentVariable.Value);
        }

        return variables;
    }

    /// <inheritdoc/>
    public string GetFolderPath(Environment.SpecialFolder folder)
    {
        return Environment.GetFolderPath(folder);
    }

    /// <inheritdoc/>
    public string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option)
    {
        return Environment.GetFolderPath(folder, option);
    }

    /// <inheritdoc/>
    public string[] GetLogicalDrives()
    {
        return Environment.GetLogicalDrives();
    }

    /// <inheritdoc/>
    public void SetEnvironmentVariable(string variable, string? value)
    {
        Environment.SetEnvironmentVariable(variable, value);
    }

    /// <inheritdoc/>
    public void SetEnvironmentVariable(string variable, string? value, EnvironmentVariableTarget target)
    {
        Environment.SetEnvironmentVariable(variable, value, target);
    }

    /// <inheritdoc/>
    public string? GetAppDirectory()
    {
        return FileSys.Path.GetDirectoryName(GetAppFileName());
    }

    /// <inheritdoc/>
    public string GetAppFileName()
    {
        return Process.GetCurrentProcess().MainModule?.FileName
            ?? Environment.GetCommandLineArgs().FirstOrDefault(string.Empty);
    }

    /// <inheritdoc/>
    public string CommandLine => Environment.CommandLine;

    /// <inheritdoc/>
    public string CurrentDirectory
    {
        get => Environment.CurrentDirectory;
        set => Environment.CurrentDirectory = value;
    }

    /// <inheritdoc/>
    public int CurrentManagedThreadId => Environment.CurrentManagedThreadId;

    /// <inheritdoc/>
    public int ExitCode
    {
        get => Environment.ExitCode;
        set => Environment.ExitCode = value;
    }

    /// <inheritdoc/>
    public bool HasShutdownStarted => Environment.HasShutdownStarted;

    /// <inheritdoc/>
    public bool Is64BitOperatingSystem => Environment.Is64BitOperatingSystem;

    /// <inheritdoc/>
    public bool Is64BitProcess => Environment.Is64BitProcess;

    /// <inheritdoc/>
    public string MachineName => Environment.MachineName;

    /// <inheritdoc/>
    public string NewLine => Environment.NewLine;

    /// <inheritdoc/>
    public OperatingSystem OSVersion => Environment.OSVersion;

    /// <inheritdoc/>
    public int ProcessorCount => Environment.ProcessorCount;

    /// <inheritdoc/>
    public string StackTrace => Environment.StackTrace;

    /// <inheritdoc/>
    public string SystemDirectory => Environment.SystemDirectory;

    /// <inheritdoc/>
    public int SystemPageSize => Environment.SystemPageSize;

    /// <inheritdoc/>
    public int TickCount => Environment.TickCount;

    /// <inheritdoc/>
    public string UserDomainName => Environment.UserDomainName;

    /// <inheritdoc/>
    public bool UserInteractive => Environment.UserInteractive;

    /// <inheritdoc/>
    public string UserName => Environment.UserName;

    /// <inheritdoc/>
    public Version Version => Environment.Version;

    /// <inheritdoc/>
    public long WorkingSet => Environment.WorkingSet;
}

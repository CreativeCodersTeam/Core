using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.SysEnvironment;

/// <summary>
/// Provides static access to environment information by delegating to an <see cref="IEnvironment"/> implementation.
/// Serves as a testable wrapper for <see cref="Environment"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static class Env
{
    private static IEnvironment __environmentInstance = new EnvironmentWrapper();

    /// <summary>
    /// Replaces the underlying <see cref="IEnvironment"/> implementation used by all static members.
    /// </summary>
    /// <param name="environment">The replacement environment implementation.</param>
    /// <returns>An <see cref="IDisposable"/> that restores the previous implementation when disposed.</returns>
    public static IDisposable SetEnvironmentImpl(IEnvironment environment)
    {
        Ensure.IsNotNull(environment);

        var oldEnvironment = __environmentInstance;

        var resetEnvImpl = new DelegateDisposable(() => __environmentInstance = oldEnvironment, true);

        __environmentInstance = environment;

        return resetEnvImpl;
    }

    /// <summary>
    /// Terminates the current process and returns the specified exit code to the operating system.
    /// </summary>
    /// <param name="exitCode">The exit code to return to the operating system.</param>
    public static void Exit(int exitCode)
    {
        __environmentInstance.Exit(exitCode);
    }

    /// <summary>
    /// Replaces the name of each environment variable embedded in the specified string
    /// with the string equivalent of the value of the variable.
    /// </summary>
    /// <param name="name">The string containing the names of zero or more environment variables.</param>
    /// <returns>The string with each environment variable replaced by its value.</returns>
    public static string ExpandEnvironmentVariables(string name)
    {
        return __environmentInstance.ExpandEnvironmentVariables(name);
    }

    /// <summary>
    /// Immediately terminates the process with the specified message.
    /// </summary>
    /// <param name="message">The message that explains why the process was terminated, or <see langword="null"/> if no explanation is provided.</param>
    public static void FailFast(string? message)
    {
        __environmentInstance.FailFast(message);
    }

    /// <summary>
    /// Immediately terminates the process with the specified message and exception.
    /// </summary>
    /// <param name="message">The message that explains why the process was terminated, or <see langword="null"/> if no explanation is provided.</param>
    /// <param name="exception">The exception associated with the failure, or <see langword="null"/> if there is none.</param>
    public static void FailFast(string? message, Exception? exception)
    {
        __environmentInstance.FailFast(message, exception);
    }

    /// <summary>
    /// Returns the command-line arguments for the current process.
    /// </summary>
    /// <returns>An array of strings containing the command-line arguments.</returns>
    public static string[] GetCommandLineArgs()
    {
        return __environmentInstance.GetCommandLineArgs();
    }

    /// <summary>
    /// Retrieves the value of an environment variable from the current process.
    /// </summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <returns>The value of the environment variable, or <see langword="null"/> if the variable is not found.</returns>
    public static string? GetEnvironmentVariable(string variable)
    {
        return __environmentInstance.GetEnvironmentVariable(variable);
    }

    /// <summary>
    /// Retrieves the value of an environment variable from the specified location.
    /// </summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <param name="target">The location of the environment variable.</param>
    /// <returns>The value of the environment variable, or <see langword="null"/> if the variable is not found.</returns>
    public static string? GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
    {
        return __environmentInstance.GetEnvironmentVariable(variable, target);
    }

    /// <summary>
    /// Retrieves all environment variable names and their values from the current process.
    /// </summary>
    /// <returns>A dictionary containing all environment variable names and their values.</returns>
    public static IDictionary<string, object?> GetEnvironmentVariables()
    {
        return __environmentInstance.GetEnvironmentVariables();
    }

    /// <summary>
    /// Retrieves all environment variable names and their values from the specified location.
    /// </summary>
    /// <param name="target">The location of the environment variables.</param>
    /// <returns>A dictionary containing all environment variable names and their values.</returns>
    public static IDictionary<string, object?> GetEnvironmentVariables(EnvironmentVariableTarget target)
    {
        return __environmentInstance.GetEnvironmentVariables(target);
    }

    /// <summary>
    /// Returns the path of the specified system special folder.
    /// </summary>
    /// <param name="folder">The special folder to retrieve the path for.</param>
    /// <returns>The path to the specified system special folder.</returns>
    public static string GetFolderPath(Environment.SpecialFolder folder)
    {
        return __environmentInstance.GetFolderPath(folder);
    }

    /// <summary>
    /// Returns the path of the specified system special folder using the specified option.
    /// </summary>
    /// <param name="folder">The special folder to retrieve the path for.</param>
    /// <param name="option">The option for accessing the special folder.</param>
    /// <returns>The path to the specified system special folder.</returns>
    public static string GetFolderPath(Environment.SpecialFolder folder,
        Environment.SpecialFolderOption option)
    {
        return __environmentInstance.GetFolderPath(folder, option);
    }

    /// <summary>
    /// Returns the names of the logical drives on the current computer.
    /// </summary>
    /// <returns>An array of strings containing the names of the logical drives.</returns>
    public static string[] GetLogicalDrives()
    {
        return __environmentInstance.GetLogicalDrives();
    }

    /// <summary>
    /// Creates, modifies, or deletes an environment variable stored in the current process.
    /// </summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <param name="value">The value to assign to the environment variable, or <see langword="null"/> to delete the variable.</param>
    public static void SetEnvironmentVariable(string variable, string? value)
    {
        __environmentInstance.SetEnvironmentVariable(variable, value);
    }

    /// <summary>
    /// Creates, modifies, or deletes an environment variable stored in the specified location.
    /// </summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <param name="value">The value to assign to the environment variable, or <see langword="null"/> to delete the variable.</param>
    /// <param name="target">The location of the environment variable.</param>
    public static void SetEnvironmentVariable(string variable, string? value,
        EnvironmentVariableTarget target)
    {
        __environmentInstance.SetEnvironmentVariable(variable, value, target);
    }

    /// <summary>
    /// Returns the directory path of the current application.
    /// </summary>
    /// <returns>The application directory path, or <see langword="null"/> if it cannot be determined.</returns>
    public static string? GetAppDirectory()
    {
        return __environmentInstance.GetAppDirectory();
    }

    /// <summary>
    /// Returns the file name of the current application executable.
    /// </summary>
    /// <returns>The application executable file name.</returns>
    public static string GetAppFileName()
    {
        return __environmentInstance.GetAppFileName();
    }

    /// <summary>
    /// Gets the command line for the current process.
    /// </summary>
    public static string CommandLine => __environmentInstance.CommandLine;

    /// <summary>
    /// Gets or sets the fully qualified path of the current working directory.
    /// </summary>
    public static string CurrentDirectory
    {
        get => __environmentInstance.CurrentDirectory;
        set => __environmentInstance.CurrentDirectory = value;
    }

    /// <summary>
    /// Gets the unique identifier for the current managed thread.
    /// </summary>
    public static int CurrentManagedThreadId => __environmentInstance.CurrentManagedThreadId;

    /// <summary>
    /// Gets or sets the exit code of the process.
    /// </summary>
    public static int ExitCode
    {
        get => __environmentInstance.ExitCode;
        set => __environmentInstance.ExitCode = value;
    }

    /// <summary>
    /// Gets a value indicating whether the common language runtime is shutting down.
    /// </summary>
    public static bool HasShutdownStarted => __environmentInstance.HasShutdownStarted;

    /// <summary>
    /// Gets a value indicating whether the current operating system is a 64-bit operating system.
    /// </summary>
    public static bool Is64BitOperatingSystem => __environmentInstance.Is64BitOperatingSystem;

    /// <summary>
    /// Gets a value indicating whether the current process is a 64-bit process.
    /// </summary>
    public static bool Is64BitProcess => __environmentInstance.Is64BitProcess;

    /// <summary>
    /// Gets the NetBIOS name of the local computer.
    /// </summary>
    public static string MachineName => __environmentInstance.MachineName;

    /// <summary>
    /// Gets the newline string defined for the current environment.
    /// </summary>
    public static string NewLine => __environmentInstance.NewLine;

    /// <summary>
    /// Gets an <see cref="OperatingSystem"/> object that contains the current platform identifier and version number.
    /// </summary>
    public static OperatingSystem OSVersion => __environmentInstance.OSVersion;

    /// <summary>
    /// Gets the number of processors available to the current process.
    /// </summary>
    public static int ProcessorCount => __environmentInstance.ProcessorCount;

    /// <summary>
    /// Gets the current stack trace information.
    /// </summary>
    public static string StackTrace => __environmentInstance.StackTrace;

    /// <summary>
    /// Gets the fully qualified path of the system directory.
    /// </summary>
    public static string SystemDirectory => __environmentInstance.SystemDirectory;

    /// <summary>
    /// Gets the amount of memory for an operating system memory page, in bytes.
    /// </summary>
    public static int SystemPageSize => __environmentInstance.SystemPageSize;

    /// <summary>
    /// Gets the number of milliseconds elapsed since the system started.
    /// </summary>
    public static int TickCount => __environmentInstance.TickCount;

    /// <summary>
    /// Gets the network domain name associated with the current user.
    /// </summary>
    public static string UserDomainName => __environmentInstance.UserDomainName;

    /// <summary>
    /// Gets a value indicating whether the current process is running in user interactive mode.
    /// </summary>
    public static bool UserInteractive => __environmentInstance.UserInteractive;

    /// <summary>
    /// Gets the user name of the person who is associated with the current thread.
    /// </summary>
    public static string UserName => __environmentInstance.UserName;

    /// <summary>
    /// Gets a <see cref="System.Version"/> object that describes the major, minor, build, and revision numbers of the common language runtime.
    /// </summary>
    public static Version Version => __environmentInstance.Version;

    /// <summary>
    /// Gets the amount of physical memory mapped to the process context, in bytes.
    /// </summary>
    public static long WorkingSet => __environmentInstance.WorkingSet;
}

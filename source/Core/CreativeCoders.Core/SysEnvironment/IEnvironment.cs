using System;
using System.Collections.Generic;

#nullable enable

namespace CreativeCoders.Core.SysEnvironment;

/// <summary>
/// Provides an abstraction over <see cref="Environment"/> for testability and substitution.
/// </summary>
public interface IEnvironment
{
    /// <summary>
    /// Terminates the current process and returns the specified exit code to the operating system.
    /// </summary>
    /// <param name="exitCode">The exit code to return to the operating system.</param>
    void Exit(int exitCode);

    /// <summary>
    /// Replaces the name of each environment variable embedded in the specified string
    /// with the string equivalent of the value of the variable.
    /// </summary>
    /// <param name="name">The string containing the names of zero or more environment variables.</param>
    /// <returns>The string with each environment variable replaced by its value.</returns>
    string ExpandEnvironmentVariables(string name);

    /// <summary>
    /// Immediately terminates the process with the specified message.
    /// </summary>
    /// <param name="message">The message that explains why the process was terminated, or <see langword="null"/> if no explanation is provided.</param>
    void FailFast(string? message);

    /// <summary>
    /// Immediately terminates the process with the specified message and exception.
    /// </summary>
    /// <param name="message">The message that explains why the process was terminated, or <see langword="null"/> if no explanation is provided.</param>
    /// <param name="exception">The exception associated with the failure, or <see langword="null"/> if there is none.</param>
    void FailFast(string? message, Exception? exception);

    /// <summary>
    /// Returns the command-line arguments for the current process.
    /// </summary>
    /// <returns>An array of strings containing the command-line arguments.</returns>
    string[] GetCommandLineArgs();

    /// <summary>
    /// Retrieves the value of an environment variable from the current process.
    /// </summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <returns>The value of the environment variable, or <see langword="null"/> if the variable is not found.</returns>
    string? GetEnvironmentVariable(string variable);

    /// <summary>
    /// Retrieves the value of an environment variable from the specified location.
    /// </summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <param name="target">The location of the environment variable.</param>
    /// <returns>The value of the environment variable, or <see langword="null"/> if the variable is not found.</returns>
    string? GetEnvironmentVariable(string variable, EnvironmentVariableTarget target);

    /// <summary>
    /// Retrieves all environment variable names and their values from the current process.
    /// </summary>
    /// <returns>A dictionary containing all environment variable names and their values.</returns>
    IDictionary<string, object?> GetEnvironmentVariables();

    /// <summary>
    /// Retrieves all environment variable names and their values from the specified location.
    /// </summary>
    /// <param name="target">The location of the environment variables.</param>
    /// <returns>A dictionary containing all environment variable names and their values.</returns>
    IDictionary<string, object?> GetEnvironmentVariables(EnvironmentVariableTarget target);

    /// <summary>
    /// Returns the path of the specified system special folder.
    /// </summary>
    /// <param name="folder">The special folder to retrieve the path for.</param>
    /// <returns>The path to the specified system special folder.</returns>
    string GetFolderPath(Environment.SpecialFolder folder);

    /// <summary>
    /// Returns the path of the specified system special folder using the specified option.
    /// </summary>
    /// <param name="folder">The special folder to retrieve the path for.</param>
    /// <param name="option">The option for accessing the special folder.</param>
    /// <returns>The path to the specified system special folder.</returns>
    string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option);

    /// <summary>
    /// Returns the names of the logical drives on the current computer.
    /// </summary>
    /// <returns>An array of strings containing the names of the logical drives.</returns>
    string[] GetLogicalDrives();

    /// <summary>
    /// Creates, modifies, or deletes an environment variable stored in the current process.
    /// </summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <param name="value">The value to assign to the environment variable, or <see langword="null"/> to delete the variable.</param>
    void SetEnvironmentVariable(string variable, string? value);

    /// <summary>
    /// Creates, modifies, or deletes an environment variable stored in the specified location.
    /// </summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <param name="value">The value to assign to the environment variable, or <see langword="null"/> to delete the variable.</param>
    /// <param name="target">The location of the environment variable.</param>
    void SetEnvironmentVariable(string variable, string? value, EnvironmentVariableTarget target);

    /// <summary>
    /// Returns the directory path of the current application.
    /// </summary>
    /// <returns>The application directory path, or <see langword="null"/> if it cannot be determined.</returns>
    string? GetAppDirectory();

    /// <summary>
    /// Returns the file name of the current application executable.
    /// </summary>
    /// <returns>The application executable file name.</returns>
    string GetAppFileName();

    /// <summary>
    /// Gets the command line for the current process.
    /// </summary>
    string CommandLine { get; }

    /// <summary>
    /// Gets or sets the fully qualified path of the current working directory.
    /// </summary>
    string CurrentDirectory { get; set; }

    /// <summary>
    /// Gets the unique identifier for the current managed thread.
    /// </summary>
    int CurrentManagedThreadId { get; }

    /// <summary>
    /// Gets or sets the exit code of the process.
    /// </summary>
    int ExitCode { get; set; }

    /// <summary>
    /// Gets a value indicating whether the common language runtime is shutting down.
    /// </summary>
    bool HasShutdownStarted { get; }

    /// <summary>
    /// Gets a value indicating whether the current operating system is a 64-bit operating system.
    /// </summary>
    bool Is64BitOperatingSystem { get; }

    /// <summary>
    /// Gets a value indicating whether the current process is a 64-bit process.
    /// </summary>
    bool Is64BitProcess { get; }

    /// <summary>
    /// Gets the NetBIOS name of the local computer.
    /// </summary>
    string MachineName { get; }

    /// <summary>
    /// Gets the newline string defined for the current environment.
    /// </summary>
    string NewLine { get; }

    /// <summary>
    /// Gets an <see cref="OperatingSystem"/> object that contains the current platform identifier and version number.
    /// </summary>
    OperatingSystem OSVersion { get; }

    /// <summary>
    /// Gets the number of processors available to the current process.
    /// </summary>
    int ProcessorCount { get; }

    /// <summary>
    /// Gets the current stack trace information.
    /// </summary>
    string StackTrace { get; }

    /// <summary>
    /// Gets the fully qualified path of the system directory.
    /// </summary>
    string SystemDirectory { get; }

    /// <summary>
    /// Gets the amount of memory for an operating system memory page, in bytes.
    /// </summary>
    int SystemPageSize { get; }

    /// <summary>
    /// Gets the number of milliseconds elapsed since the system started.
    /// </summary>
    int TickCount { get; }

    /// <summary>
    /// Gets the network domain name associated with the current user.
    /// </summary>
    string UserDomainName { get; }

    /// <summary>
    /// Gets a value indicating whether the current process is running in user interactive mode.
    /// </summary>
    bool UserInteractive { get; }

    /// <summary>
    /// Gets the user name of the person who is associated with the current thread.
    /// </summary>
    string UserName { get; }

    /// <summary>
    /// Gets a <see cref="System.Version"/> object that describes the major, minor, build, and revision numbers of the common language runtime.
    /// </summary>
    Version Version { get; }

    /// <summary>
    /// Gets the amount of physical memory mapped to the process context, in bytes.
    /// </summary>
    long WorkingSet { get; }
}
